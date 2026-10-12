# Інструкції для ШІ — UniTools Build

Цей файл самодостатній для роботи з пакетом. Додаткові правила репозиторію-споживача діють, коли пакет підключено до нього. Перед змінами прочитайте [README.md](README.md), [package.json](package.json) і `Editor/UniTools.Build.asmdef`.

## Призначення та сумісність

- UPM ID `com.unitools.build`, версія `0.1.10-preview`, assembly і namespace `UniTools.Build`. Assembly містить лише Editor-код; не додавайте її до runtime/player assemblies.
- Пакет — послідовний pipeline із `BuildStep` ScriptableObjects, параметрами, платформними build-кроками, IO та CLI. Він не є gameplay-системою і не залежить від VContainer, Commands, BehaviorTree чи конкретної гри.
- `package.json` зберігає upstream minimum Unity `2019.1`; перевірена версія після рев'ю — `6000.6.3f1`. Зберігайте version guards для новіших API. Компіляція Unity 6 не доводить сумісність старих Editors.
- Немає зовнішніх UPM-залежностей. Потрібні стандартні Editor/IMGUI/UIElements та .NET API. iOS-код із `UnityEditor.iOS.Xcode` компілюється лише за `UNITY_IOS` і потребує встановленого iOS module та активного iOS target. CLI має macOS/Linux shell і Windows cmd; перевірка на macOS не доводить роботу Windows/Linux.
- Реальні builds потребують відповідного Unity module. Archive/Export потребують macOS, Xcode, сертифікатів і provisioning profiles; інсталяція пакету не встановлює їх і не дозволяє публікацію.

## Навігація

| Шлях | Власник функції |
|---|---|
| `Editor/Core/Pipelines/` | `BuildPipeline`, послідовність і Project Settings UI |
| `Editor/Core/Steps/` | `BuildStep.Execute(): Task`, `UnityBuildStep`, build options |
| `Editor/Core/Parameters/` | string/int assets, CLI overrides та Inspectors |
| `Editor/BatchMode/` | аргументи, `BatchModeBuilder.Execute`, вихід CI та resume після reload |
| `Editor/Platforms/` | Android, iOS, Standalone, WebGL |
| `Editor/IO/` | шляхи, копіювання, видалення, ZIP |
| `Editor/Defines/`, `Editor/Versioning/` | зміни ProjectSettings та версії |
| `Editor/CliTools/`, `Editor/UnityEnvironment/` | процеси, discovery через PATH, shell quoting |
| `BuildScripts/Unity-Hub/` | Bash/PowerShell/bat wrappers, запускаються з кореня Unity-проєкту |

## Контракти

- Зберігайте public API, assembly identity, назви serialized fields, enum values та кожен наявний `.meta` GUID. Переміщуйте файл разом із його `.meta`; новому Unity-файлу потрібен новий унікальний GUID. Версію пакету не змінюйте без узгодженої версійної задачі.
- Нова операція — окремий `BuildStep`, конфігурація — serialized fields або параметр-asset. Custom step повертає `Task`, який завершується лише після всієї роботи. Помилки й ненульові exit codes мають перервати pipeline; не приховуйте їх логом або успішним Task.
- `BuildPipeline.Run` призначений для Editor; `RunStep` — також для batch runner. Порожній pipeline, null step, неправильний індекс — помилки конфігурації. `Skip` — явне рішення автора; воно логуються без warning. Кроки виконуються послідовно; один asset не можна запускати повторно, поки працює `Run`.
- Не використовуйте blocking `.Wait()` / `.GetAwaiter().GetResult()` на незавершеному Task у Unity main thread. `async void` дозволений лише для Unity menu/UI callbacks із catch; API операції повертає Task. Batch runner читає Task результат тільки після `IsCompleted`.
- Batch runner сам викликає `EditorApplication.Exit(0/1)`. **Не додавайте `-quit`** до цього executeMethod. Pipeline визначається asset path або унікальним exact name; дублікати імен відхиляються. Аргументи читаються через OS-tokenized argv; string parser лишається для public API параметрів.
- Прогрес batch зберігається в `SessionState`: asset GUID та наступний індекс. Після синхронного define-changing step runner чекає compilation/reload і продовжує наступним кроком. Custom async step не повинен сам запускати domain reload посеред незавершеного Task; розділіть такі дії на окремі кроки. Це resume в межах процесу Editor, а не після crash/restart.
- Немає автоматичного скасування чи rollback виконаних кроків. Editor pipeline, що змінює defines із domain reload, запускайте batch-способом. Не обіцяйте паралельне виконання Unity builds або відновлення ProjectSettings після pipeline.
- Build options включають лише enabled scenes. Успіх Unity build — лише `BuildResult.Succeeded`; Failed, Cancelled, Unknown або exception переривають pipeline.
- `CommandLine.Execute` приймає навмисну shell-команду, а не масив argv. Літеральні шляхи/значення обгортайте `CommandLine.QuoteArgument`; raw extra arguments є довіреною конфігурацією автора. stdout і stderr читаються одночасно, process звільняється. Сам CLI API синхронний і не має timeout/cancellation — довгі custom CLI steps повинні явно визначити власну політику.
- `IsInstalled` означає існування файлу за абсолютним шляхом; це не доказ працездатності, виконуваності чи наявності ліцензії. Discovery бере перший наявний executable із окремого output line. PATH доповнюється через `Path.PathSeparator`, без substring-порівняння.
- IO paths перевіряйте **до** видалення призначення. Джерело й призначення копії не можуть збігатися або бути вкладеними одне в одне. ZIP створюється .NET API поза деревом джерела, включає hidden files, замінює попередній archive. Валідація шляхів лексична; автор відповідає за symlink aliases та дозволені build-output directories.
- Signing/defines/version/platform steps навмисно змінюють ProjectSettings. Не запускайте на споживачі неперевірений pipeline. Паролі не логувати, не записувати в документацію/fixtures; для CI постачайте їх через авторизовані локальні або secret inputs.
- Version steps працюють із `System.Version` (2–4 невід’ємні числові компоненти), не з повним SemVer/prerelease. Increment не скидає інші компоненти; missing build/revision нормалізується в 0 перед increment; overflow — помилка.
- Не приховуйте compiler warnings широким `#pragma warning disable`. Вузький CS0414 біля iOS serialized fields пояснюється тим, що Inspector зберігає їх на інших targets.

## Перевірки та документація

1. Скомпілюйте **весь пакет** через Unity Editor і перевірте compiler warnings/errors, зокрема при Android та iOS active targets. Не редагуйте `Library`, `Temp`, generated `.csproj` або compiler output.
2. Перевірте standalone мінімальний host із локальним UPM entry `"com.unitools.build": "file:<absolute-package-path>"`, без game packages. Пакет не має власного Test Framework чи збереженої test suite; використовуйте тимчасові C# probes/Editor Test Runner споживача, не підміняйте виконання статичним пошуком.
3. Для змінених контрактів перевірте порядок/await, fail-stop, explicit Skip/null steps, quoted/empty/negative CLI values, великий stderr, ненульовий exit, відсутній tool, paths із пробілами, invalid copy paths і ZIP overwrite/hidden files. Fixtures і output створюйте лише в disposable temporary directory та прибирайте після перевірки.
4. Реальний batch launch має завершитися з 0 на успіх, 1 на відсутній pipeline/failed step. Перевірте asynchronous delay та define-changing step → reload → наступний крок. Scripts мають передати точні аргументи й exit code; Bash: `bash -n BuildScripts/Unity-Hub/{build,unity}.sh`. PowerShell/Windows перевіряйте на їхньому host.
5. У споживачі: UPM resolve, compiler, відповідні наявні tests; перевірте, що `UniTools.Build` відсутній у player assemblies. Build artifact, player-script compile і device QA — різні докази.
6. Результати, версію Editor, фактичні межі та зміну поведінки записуйте в наявному README. Не додавайте окремі review/report файли чи Python verification tools. Не очищайте Console, щоб приховати діагностику.

## Git та дозволи

- Це окремий upstream Git repository. Git state/index/history/remotes/config, commit/push, LFS і submodule operations потребують явного дозволу користувача на відповідні дії. Дозвіл минулого review не діє на майбутні задачі.
- Read-only Git запускайте з `GIT_OPTIONAL_LOCKS=0`. Звичайні source edits у межах задачі дозволені.
- Package commit і host gitlink — окремі дії. Локальні fixes у dirty submodule **не входять** до його pinned commit і не відтворюються на fresh clone. Перед передачею явно повідомте цей стан; не pin-ьте unpublished commit без доступного remote.
- За дозволеного commit використовуйте English message та `Stanislav Felinskyi <felinskyi.sg@gmail.com>` для author і committer. Перед будь-якою публікацією звірте обидві identities. Review/integration не є дозволом публікувати build, створювати release/tag або писати повідомлення третім особам.
