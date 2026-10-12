# UniTools Build

Editor-пакет для послідовної збірки Unity-проєктів через ScriptableObject-кроки. UPM `com.unitools.build` `0.1.10-preview`, namespace/assembly `UniTools.Build`, MIT. Кроки можна виконувати з Editor або batch mode. Інструкції для ШІ — [AGENTS.md](AGENTS.md).

## Інсталяція

Звичайний Git UPM entry:

```json
"com.unitools.build": "https://github.com/UniToolsTeam/unitools-build.git#master"
```

Для контрольованої локальної копії додайте SSH-сабмодуль у корені **Unity-проєкту** з дозволом власника Git:

```sh
git submodule add -b master git@github.com:UniToolsTeam/unitools-build.git Packages/com.unitools.build
```

Якщо Unity-проєкт — підкаталог іншого repository, prefix шляху додається від кореня repository. UPM entry замінюється на:

```json
"com.unitools.build": "file:com.unitools.build"
```

Submodule фіксує конкретний commit; `branch = master` не оновлює його автоматично. Fresh clone потребує дозволеного `git submodule update --init --recursive`. SSH account/host alias налаштовує власник машини, не пакет.

Зовнішніх UPM-залежностей немає. Код лише Editor; iOS Xcode API потребує iOS module та активного iOS target. Upstream minimum у manifest — Unity `2019.1`; поточна перевірка — `6000.6.3f1`, старі версії повторно не перевірені. Для builds потрібні platform modules; для iOS archive/export — macOS, Xcode та signing credentials.

## Pipeline та параметри

Створіть asset через **Create → UniTools → Pipeline**, додайте `BuildStep` assets у `m_steps` у потрібному порядку. **Context Menu → Run** або **Project Settings → UniTools → Build → Run** запускає його. Успішний крок повністю завершує свій Task перед наступним; помилка зупиняє pipeline. Порожні/null кроки — configuration errors, `Skip` — явний пропуск. Один pipeline asset не запускається повторно під час `Run`.

Немає окремих Pre/Post base classes: успадковуйте **`BuildStep`**, додавайте операції до одного впорядкованого списку. Наприклад:

```csharp
using System.Threading.Tasks;
using UniTools.Build;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/Wait")]
public sealed class WaitBuildStep : BuildStep
{
    [SerializeField] private int m_milliseconds = 250;
    public override Task Execute() => Task.Delay(m_milliseconds);
}
```

`UnityBuildStepWithOptions` створює BuildPlayerOptions з enabled scenes та serialized output/development/clean-cache settings. Готові steps: APK/AAB, macOS/Windows, iOS Xcode, WebGL; archive/export/upload; signing; defines; numeric version increment; paths/copy/delete/ZIP. Тільки `BuildResult.Succeeded` є успіхом. Pipeline не виконує автоматичний rollback: signing/version/defines та platform settings залишаються зміненими.

`StringBuildParameter`, `IntBuildParameter`, `ScriptingDefineSymbols` можна перевизначити в batch. Ключ — назва asset без пробілів, lowercase invariant, із `--`; наприклад `Release Notes` → `--releasenotes`. Значення з пробілами обгортайте лапками. Підтримано `--key=value`, empty string і від’ємні числа; malformed integer — помилка. Ключі параметрів мають бути унікальними й не конфліктувати з `--pipeline` або Unity arguments.

Version steps використовують `System.Version`, 2–4 числові компоненти, без SemVer suffixes; increment зберігає інші компоненти.

## Batch mode

Із кореня Unity-проєкту:

```sh
/path/to/Unity -batchmode -projectPath "$PWD" \
  -executeMethod UniTools.Build.BatchModeBuilder.Execute \
  --pipeline "Assets/Build/Release Pipeline.asset" -logFile -
```

Можна передати унікальне exact asset name замість path. Дублікати імен відхиляються. Runner завершує Editor з **exit 0** лише після всіх Tasks, із **exit 1** при помилці. **Не передавайте `-quit`** цьому entry point: його asynchronous completion потребує власного lifetime. Загальні правила `-executeMethod`, `-batchmode` та завершення Editor описані в [Unity CLI reference](https://docs.unity3d.com/6000.0/Documentation/Manual/EditorCommandLineArguments.html).

Runner зберігає GUID pipeline та наступний крок у SessionState; після синхронного `ApplyDefines` чекає compilation/domain reload і продовжує. Custom async step не повинен сам перезавантажувати assemblies до завершення Task. Editor Run для pipeline, який змінює defines, замініть batch-запуском. Recovery після crash/restart та cancellation не реалізовані; timeout у CI має задавати зовнішній runner.

Wrappers у `BuildScripts/Unity-Hub/` можна запускати без копіювання, але working directory має бути коренем Unity-проєкту:

```sh
bash Packages/com.unitools.build/BuildScripts/Unity-Hub/build.sh \
  "/path with spaces/to/Unity" --pipeline "Release Pipeline"
```

`build.sh` визначає installed Hub Editor через ProjectVersion.txt; optional first argument або `UNITOOLS_UNITY_PATH` задає executable. `unity.sh` — generic wrapper, optional first argument — executable або Unity version; решта аргументів передаються без змін. PowerShell `build.ps1`/`unity.ps1` і `build.bat` використовують Windows Hub path чи `UNITOOLS_UNITY_PATH`, чекають завершення process і повертають його exit code. PowerShell build пише `PipelineResults.log`.

## CLI

**Project Settings → UniTools → CLI** показує зареєстровані tools; **Tools → CLI → UnityEnvironment** — environment, успадкований Editor. `[assembly: PathVariable("/your/tool/directory")]` доповнює PATH process, зі стандартним separator. Не додавайте machine paths до reusable package.

```csharp
ToolResult result = CommandLine.Default.Execute("echo Hello");
if (result.ExitCode != 0) throw new BuildStepFailedException(result.Error);
```

`CommandLine.Execute` виконує shell-команду синхронно. stdout/stderr читаються одночасно; довга команда все одно блокує caller, timeout/cancellation не вбудовані. Літеральне значення/шлях обгортайте `CommandLine.QuoteArgument(value)`. Raw command strings та `Archive.m_extraArgs` мають бути довіреною конфігурацією. Windows cmd має власні правила expansions; QuoteArgument не робить довільний shell input безпечним.

`Cli.Tool<T>()` повертає перший зареєстрований tool цього типу або null; `Cli.Tool(name)` потребує exact registered executable key. Готові tools — shell, path resolver, Tar, Zip, XCodeBuild (реєструється лише на iOS target). `IsInstalled` перевіряє існування файлу, не його успішний запуск.

Custom tool: успадкуйте `BaseCliTool`, створіть `BaseCliToolAttribute` із `Create()` та додайте assembly attribute. Generic tool: `[assembly: CliTool("executable")]` або `[assembly: CliToolPath("executable", "/absolute/path")]`. Shell paths із пробілами підтримані. Перевіряйте `ToolResult.ExitCode` у кожному build step.

IO копії перевіряють джерело до видалення призначення й відхиляють overlap. ZIP step використовує .NET, включає hidden files і створює archive поза джерелом. `m_archiveFileName` може бути абсолютним або відносним до source folder, наприклад `../output.zip`. Output усередині source відхиляється; попередній ZIP замінюється, а не доповнюється stale entries. Symlink aliases потребують авторської перевірки.

## Рев'ю та перевірки — 12.10.2026

Рев'ю від upstream `master`, commit `0e8f482178de0c3b2a8c8ff0c6631d0631cc3417`: виправлено async callbacks/CI exit та reload continuation, parsing/quoting, process pipe deadlock/disposal, PATH, build result/disabled scenes, null конфігурацію, copy overlap, ZIP, iOS plist parsing та obsolete Unity APIs. Public type/assembly identities, version і original GUIDs збережені. Застарілі README приклади Pre/Post classes замінені actual API.

Перевірено Unity `6000.6.3f1` на macOS:

- Host compile та `dotnet build UniTools.Build.csproj --no-restore` (.NET SDK `10.0.401`) — **0 warnings / 0 errors**. У поточному Unity Console також 0/0; історія transient compiler/Pipeline diagnostics збережена.
- Мінімальний host лише з цим пакетом, без game/Test Framework dependencies: Standalone Editor compile — **0 compiler warnings/errors** після виправлення UAC0005. Реальний `BuildMacOS` через batch створив `Smoke.app` (72 MB), exit **0**; disabled scene із неіснуючим path виключена з build options.
- **51/51** temporary behavioral checks: awaited порядок/зупинка/skip/null/concurrent Run, parser/parameters, IO overlap, ZIP hidden files і overwrite, shell quoting, missing executables, stdout/stderr та 128 KiB stderr без deadlock, PATH. **16/16** iOS checks: provisioning XML, перший TeamIdentifier, invalid input, archive/export arguments із пробілами та літеральними shell-символами, fake Xcode failure/missing tool.
- **10/10** Bash wrapper checks: exact argv із пробілами, optional executable/version, log override, exit propagation; обидва scripts проходять `bash -n` на стандартному macOS Bash.
- Реальні batch launches: async success → **0**; failed step → **1**, наступний marker не виконується; missing pipeline → **1**; ApplyDefines → compilation/domain reload → наступний step → **0**, before/after виконані по одному разу.
- Споживач game-1: **217/217 Prototype1.Core.Tests**, 0 failed/skipped; Android (**50 assemblies**) та iOS (**48**) player scripts compile із `Prototype1.Core`/`Prototype1.Runtime` і без `UniTools.Build`/test assemblies. IDE projects перегенеровані через Unity.
- Усі **201 original .meta** побайтово ідентичні upstream; package version та assembly GUID незмінні.

Windows/PowerShell, Linux та старі Unity Editors не запускалися. iOS CLI checks використовували локальний fake executable: signing credentials, App Store upload та device QA не виконувалися. Build artifact мінімального host і compile player scripts споживача не є mobile release гри. Temporary fixtures/outputs не входять до package.

Виправлення збережено окремим review commit поверх upstream master; Git state споживача фіксує його через gitlink. Інтеграційний дозвіл 12.10.2026 не дозволяє майбутні Git-операції.
