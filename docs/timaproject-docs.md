Приложение для сохранения временных промежутков времени (records) с использованием встроенного таймера.

## Use cases
![](./use-cases/images/timaproject-use-case.svg)

- Пользователь может запустить таймер с введенным заголовком, проектом или значениями по умолчанию и отсчетом от текущего времени

- Пользователь останавливает таймер, все поля таймера обнуляются. Record становится завершенным и получает EndTime от текущего времени
Обнуление полей: title - пустой, project - пустой, time - 00:00:00

- Во время работы таймера возможно изменить время начала отсчета, графа с временем окончания недоступна

- Пользователь может изменять завершенную запись. Изменять время, проект, заголовок, добавлять заметки к записи

- При выборе проекта к record в окне появится список из текущих проектов и "пустого" проекта - проекта по умолчанию. Также пользователь может ввести новый проект и запись автоматически изменит свой проект на новый.

- У пользователя в отдельном окне есть список проектов, где можно изменить имя проекта и удалить проект

### Настроить проект у Record
Пользователь может выбрать


За настройку проекта будет отвечать ProjectForm - модульное окно, которое содержит список проектов с выбранным проектом. При клике на проект окно закрывает, а проект Record меняется на выбранный. Также в ProjectForm есть возможность добавить проект новый проект, при добавлении проект автоматически выбирается

#### Добавить заметку к записи

К завершенной Record пользователь может добавлять Заметки Note, заметок может быть неограниченное количество

- Добавление Notes к записям


## Use Case дополнительные

- Создание интерфейса для плагина

- Плагин для синхронизации с обсидианом

- Импорт данных из toggl

- Настройка автозапуска

-Настройка Date

- Визуальное оформление Project

- Инструменты статистики

- Умная строка
В таймере должны предлагаться название заголовков для Record


> Использование Date
Поле Date представляет собой дату с которая ассоциирована TimaNote. Она нужна для того чтобы ассоциировать записи после полуночи (когда фактически наступил новый день) с предыдущим днем. 

## Реализация use cases

### Устройство Record
Record - класс уровня модели данных

Обязательные для назначения свойства:
DateTime StartTime время начала записи
Guid Id - id записи
DateOnly Date - дата, к которой привязана запись

Свойства с параметрами по умолчанию:
string Ttile - заголовок, по умолчанию пустой
Project Project - проект, по умолчанию Project.Empty
List<Note> Notes - заметки, по умолчанию пустые
DateTime? EndTime - время окончания записи, по умолчанию null

Read-only свойства
bool IsActive - завершенность записи, false, если EndTime =null

ITimeBase - интерфейс для свойств связанный со временем

IRecordViewModelBase : ITimeBase

IEditRecord - интерфейс команд для открытия форм, которые редактируют IRecordViewModelBase

EditableRecordViewModel наследует NotifyDataErrorViewModel и реализует интерфейсы IRecordViewModelBase, IEditRecord

EditableRecordViewModel сохраняет все валидные изменения над Record в репозиторий. EditableRecordViewModel содержит NoteFormViewModel для работы с заметками этой record.

### Работа с заметками
EditableRecordViewModel содержит NoteFormViewModel в котором происходит редактирование новой заметки. После вызова AddNoteCommand, NoteFormViewModel добавляет новую заметку в репозиторий заметок. Обновление репозитория, обновляет репозиторий с record. И тем самым обновляет Record

EditableRecordViewModel содержит ListingNoteViewModel с заметками из Record

ListingNoteViewModel изменяет указанные Note в EditableNoteViewModel, также содержит ссылку на NoteRepository для обновления списка

EditableNoteViewModel содержит Note и свойство Text, и NoteRepository

### Timer

TimerViewModel наследует NotifyDataErrorViewModel и реализует интерфейсы IRecordViewModelBase, IEditRecord

Timer имеет два состояния: выключен (1) и включен (2). При переходе 1-2. Таймер устанавливает StartTime на текущее вермя, а Time показывает разницу между StartTime и текущим временеи. Также при запуске создается и сохраняется в репозиторий новая Record с параметрами, которые ввел пользователь до запуска.

Во время работы таймера (состояние 2). Любые изменения свойств интерфейса IRecordViewModelBase. Применяются к созданной Record и сохраняются в репозитории

Во время выключения таймер (переход 2-1). Сохраняется EndTime с текущим временем и это время сохраняется в Record. Record становится завершенной. Таймер обновляет свои значения на значения по умолчанию

### Валидация RecordViewModel
RecordValidator реализует AbstractValidator<IRecordViewModelBase>. RecordValidator содержит в себе TimeValidator

TimeValidator реализует AbstactValidator<ITimeBase>

Правила валидации:
- StartTime парсится в DateTime
- EndTime парсится в DateTime
- При выполнении условий выше EndTime > StartTime
- Time парсится в TimeSpan

### Проблема установки времени
ITimeBase
string StartTime
string EndTime
string Time

Свойство Time должно быть разностью между EndTime и StartTime. При этом изменяя Time должно изменяться StartTime относительно EndTime. Отсюда должна быть логика, которая автоматически реагирует на подобные изменения.

Если EndTime имеет некорректное значение, то Time также имеет некорректно значение.
Если EndTime имеет корректное значение, то изменение Time изменяет StartTime.
Изменение StartTime на корректное значение, изменяет Time
Изменение Time на корректное значение при корректном EndTime, меняет StartTime

За все это будет отвечать TimeSolver. TimeSolver будет иметь метод Solve, который принимает имя измененного свойства. В зависимости от свойства, значений валидатора TimeSolver будет переназначать необходимые свойства.

TimeSolver имеет два состояния. Solving и NotSolving. Пример: 
- корректно изменилось свойство Time
- вызов TimeSolver, который проверил что все свойства корректны и назнчает StartTime
- изменение StartTime вызывают TimeSolver, который при всех корректных значениях изменит Time

Алгоритм зациклился. Чтобы этого избежать при вызове TimeSolver его состояние ставится на Solving, по окончанию вызова, состояние меняется на NotSolving

### TimeForm
TimeForm форма по редатированию времени. Содержит TimeValidator, TimeSolver и IRecordViewModelBase, который редактируется. Реализует ITimeBase. TimeForm вызывается как отдельный компонент содержит Close-навигацию

### ProjectForm
Содержит список Projects, ProjectRepository, IRecordViewModelBase, и выбранный на данный момент проект SelectedProject

ProjectForm может добавлять новый проект и сразу же применять его к выбранной Record. Либо пользователь выбирает проект из списка созданных.

После выбора проекта ProjectForm закрывается. Также имеет свою close-навигацию

IProjectName - содержит свойство имени проекта, используется в ProjectForm и EditableProject

ProjectNameValidator - проверяет что имя уникально и не хранится в репозитории

Projects содержит проекты в специально контейнере ProjectFromContainer, который состоит из EditableProjectViewModel, если проект не пустой (или null если пустой), IsEmpty, IsSelected - свойства
### Репозитории
Интерфейс для репозиторием IRepository<T>, выполняет CRUD операции, также содержит событие об изменении репозитория
RepositoryChangedEventArgs<T>, которое состоит из объекта и операции совершенной с объектом RepositoryChangedOperation

Операции:
- Добавление
- Обновление
- Удаление


Реализация на модели
- Record
- Project
- Note

NotFoundException<T> - исключение, которое выбрасывается если изменяется объект, корого нет в репозитории

## Компоненты (бизнес-логика)

### EditableRecordViewModel

ITimeBase
string StartTime
string EndTime
string Time

IRecordViewModelBase : ITimeBase
string Title
Project Project
string Date

IEditRecord
ICommand OpenTimeFrom
ICommand OpenProjectForm

EditableRecordViewModel наследует NotifyDataErrorViewModel и реализует интерфейсы IRecordViewModelBase, IEditRecord

Тесты
- Метод Validate должен добавлять ошибку, если валиадатор не пропустил какое-либо из свойств RecordViewModel

- Изменение свойств класса должны вызывать валидатор

- Любые корректные изменения свойств RecordViewModel должны обновлять Record через IRepository с Record

- DeleteRecord() должен удалить Record из IRepository

- OnRepositoryUpdate по вызовы должен обновить EditableRecordViewModel новыми данными

### NoteFromViewModel
NoteFormViewModel
_noteRepository - репозиторий с заметками
CurrentText - текущий текст заметки
AddNoteCommand
AddNote(Note)

Тесты
- AddNote вызывает _noteRepository с новой Note и указанным текстом

### ListingNoteViewModel
ListingNoteViewModel

Тесты
- При заданных Note должен корректно получится список EditableNoteViewModel

- OnRepositoryUpdate обновляет заданную Note, если она была в списке

### EditableNoteViewModel
EditableNoteViewModel 

Тесты
- При изменении Text в NoteRepository вызывается Update с обновленной Note

- DeleteNote() вызывает IRepository.Delete() с указанной заметкой

- OnRepositoryUpdate - при обновлении репозитория с указанной заметкой, обновляются text и Note

### TimerViewModel
enum TimerState {
    NotRunning,
    Running
}

string Time показывает отчет таймера, по умолчанию "0:00:00"
_dispatherTimer - запускает background task при запуске таймера
TimerCommand - команда запуска\остановки таймера


Тесты:
- При запуске таймера создается Record и добавляется в репозиторий
- Record должен иметь StartTime - из поля StartTime, а EndTime - null
- Record должен принимать Title из формы Title или принимать значение по умолчанию
- Record принимает Project из поля Project

- Изменение StartTime должно StartTime в репозитории
- Изменение Title должно изменять Title в репозитории
- Изменение Date должно изменять Date в репозитории

### TimeValidator
Тесты 
- StartTime парсится в DateTime (1)
- EndTime парсится в DateTime (2)
- При выполнении условий выше EndTime > StartTime (3)
- Time парсится в TimeSpan (4)
- Если (2) не выполнено, то Time не валидируется

### TimeSolver
Тесты
...

### TimeForm

Тесты
- При изменении свойств StartTime, EndTime, Time на корректные, изменяются соответсующие свойства в RecordViewModel

---

При клике на поле должна выходить форма. Форма содержит четыре поля Time, StartTime, EndTime, Date. 

Time является разностью времени EndTime и StartTime

Time при измнении изменяет поле StartTime. 
Тесты (юнит):
- Ввод при инициализации Time должен быть пустой строкой 

- При инициализации Time не содержать ошибки

- Если пустая строка в Time назначается во второй раз, то это приводит к ошибке

- Неправильный ввод Time должен приводить к ошибки по данному полю

- Если IsEndTimeEnabled false, EndTime устанавливает текущее время

- Если IsEndTimeEnabled false, то EndTime не изменяет своего значения

- Если StartTime изменил значение, а EndTime и Time пустые, то они не изменяются

- Если EndTime изменил значение, а StartTime и Time пустые, то они не изменяются

- Если измененные пустые значения ставятся дважды, то они вызывают ошибку в Time

- Если Time корректно изменило значение, но EndTime некорректное, то Time должно иметь ошибку

- Если Time корректно изменило значение и EndTime корректное, то оно должно менять StartTime

- Если Time некорректное, то StartTime не меняет значения

- Если EndTime некорректное, то EndTime не меняет значения

- Если форма редактирует активный Record, то EndTime устанавливается на время открытия формы и недоступно для редактирования




### ProjectFormViewModel
Тесты
- выполнение AddProjectCommand, если нет ошибок валидации, должен создать новый проект с именем из Name и отправить его в репозиторий на добавление
- выполнение AddProjectCommand, если нет ошибок валидации, присваивает новый проект IRecordViewModel
- выполнение AddProjectCommand, если нет ошибок валидации, закрывает ProjectFormViewModel
- выполнение AddProjectCommand, если есть ошибка валидации, вызывает исключение AddingInvalidProjectException

- Name которой назначили пустое имя должна содержать ошибку валидации
- Name которой назначили имя, которое существует в репозитории должна содержать ошибку валидации

- По умолчанию IsCanAdd false, Name - пустое
- Если новое значение Name не проходит валидацию IsCanAdd = false
- Если новое значение Name проходит валидацию IsCanAdd = true

- После инициализации репозиторий возвращает все проекты из ProjectRepository
- Projects должен содержать все проекты из ProjectRepository, а также пустой проект (сортировка - сначала пустой, а потом а алфавитном порядке)
- Проект из IRecordViewModel должен быть выделен в Projects

- SelectProjectCommand присваивает проект из CommandParameter в IRecordViewModel
- SelectProjectCommand закрывает ProjectForm

- Добавление проекта в репозиторий, обновляет Projects в добавленным проектом
- Обновление проекта в репозитории, обновляет Projects и обновленным проект


### ProjectFromContainer
ProjectViewModel
IsEmpty
IsSelected

### EditableProjectViewModel
Тесты
- Если Name присваивает корректное имя, Project присваивает имя и вызывается сохранение проекта с обновленным именем в базе
- Если Name присваивается некорректное имя, HasError станится true, HarPropertyError содержит ошибку с сообщением, репозиторий не вызывается
- Remove() вызывает Remove в репозитории с данным проектом
- RemoveProjectCommand должна вызывать Remove()
- Добавление пустого проекта должно вызывать exception
- При инициалцизации Name EditableProjectViewModel и Name Project должно совпадать
- OnRepositoryChanged если изменился данный проект, должен обновить имя

### ProjectNameValidator
Тесты
- Валидатор не должен пропускать IProjectName если проект с таким именем содержится в репозитории
- Имя IProjectName не должно быть пустым
### IRepository

AddItem(T item)
UpdateItme(T item)
DeleteItme(T item)
Contains(T item)
GetItems(predicate)
event RepositoryChanged

### ProjeсtRepository
Хранилище для проектов

Тесты
- Contains должен возвращать true если проект хранится, иначе false
- AddItem должен добавлять новый проект в репозиторий
- AddItem должен выбрасывать ошибку если добавленный проект существует
- AddItem должен выбрасывать ошибку если добавленный проект с таким названием уже есть
- UpdateItem должен обновлять проект с таким же Id
- UpdateItem должен выбрасывать ошибку если проекта с таким Id нет в репозитории
- UpdateItem должен выбрасывать исключение, если изменяется пустой проект
- RemoveItem должен удалять проект и возвращать true или возвращать False если проекта не было в репозитории
- RemoveItem должен выбрасывать исключение если удаляется пустой проект
- Contains должен возвращать true если проект с таким именем есть или false если нет
- GetItems() должен возвращать все проект подходящие под предикат
- GetItems не должен возвращать пустой проект

- AddItem должен вызываеть RepositoryChanged с добавленным объектом и операцией - Adding
- AddItem не должен вызывать RepositoryChanged, если вышла ошибка
- RemoveItem должен вызываеть RepositoryChanged с удаленным объектом и операцией - Removing
- RemoveItem не должен вызывать RepositoryChanged, если объекта не было в репозитории
- UpdateItem должен вызываеть RepositoryChanged с обновленным объектом и операцией - Updating
- UpdateItem не должен вызывать RepositoryChanged, если объекта не было в репозитории


### ListingRecordViewModel
ListingRecordViewModel обращается к RecordListingStore, который содержит текущие список Record

ListingRecordViewModel подписан на изменения RecordListingStore, в случае изменения обновляет

Список должен пролистываться вниз с некоторым лимитом и подгружаться в случае необходимости


![](./class_diagrams/images/listing-store.svg)

Тесты

При инициализации ListingRecordViewModel берет записи из стора и создает из них список RecordViewModel

Должен быть подписан на событие ListingRecordStore и обновлять Records

Во время вызова события должен вызываться PropertyChanged у Records

#### ListingRecordStore
Содержит текущий список Record, изменяет этот список по заданным параметрам. За списком обращается в RecordRepository

FilterListingArgs - параметры фильтрации

Пока что попробуем объединить любые изменения в одно событие (посмотрим как будет визуально смотреться)

ListingChanged - событие уведомляющее об изменении списка
FilterChanged - событие уводомляющее что нужно заменить один список другим

Тесты:
По умолчанию ListingRecordStore получает пустой FilterListingArgs, поэтому в Records все неактивные записи

ListingRecordStore должен получить список из репозитория по указанным параметрам фильтрации

Изменение параметров фильтрации должно приводить к изменению списка в store

FilterChanged должен срабатывать когда меняются параметры фильтра и обновлять текущий список

ListingChanged должен сработать когда в репозиторий добавили новый record и он подходит под параметры фильтрации и обновить текущий список

ListingChanged должен сработать когда обновили record и он подходит под параметры фильтрации и обновить текущий список

ListingChanged должен сработать когда удалили record и он подходит под параметры фильтрации и обновить текущий список


## Хранилище данных
IRecordRepository - репозиторий с Record
GetNewId() - дает новый свободный Id для создания репозитория

AddRecord(Record) - добавить новую Record

DeleteRecord(Record) - возвращает true если запись была найдена и удалена и false если не найдена

Contains(Record) - проверяет содржится ли Record в репозитории

UpdateRecord(Record) - обновляет Record

GetAllRecords(predicate) - дает все записи удовлетворяющие предикату, по умолчанию дает все записи

GetRecords(FilterListingArgs) - дает все записи удовлетворящие фильтру

RecordsChanged - вызывается когда изменяется репозиторий

### RecordRepository

Тесты
- GetNewId - должен давать Id, которого нет в хранилище
- AddNewRecord  должен добавлять новую Record
- AddNewRecord должен выбрасывать Exception, если Record с таким Id существует
- UpdateRecord - должен обновлять Record, если она есть в репозитории
- UpdateRecord должен выбрасывать исключение, если Record нет в репозитории

- GetRecords должен выдавать все Record EndTime которых раньше From
- GetRecords должен выдавать все Record EndTime которых позже To
- GetRecords должен выдвать все Record с заданным проектами
- GetRecords должен выдавать нужное количество элементов по убывания свойства EndTime
- GetRecords должен выдавать активные Record, если так задан FilterArgs
- GetRecords должен выдавать все неактивные Record, если FliterArgs пустой

- RecordsChanged вызывается когда добавляется новый Record

#### RepositoryChangedEventArgs
- RepositoryChangedOperation операция по изменению списка
    - Add добавление элемента
    - Delete удаление
    - Update обновление
- Record с которым производится операция

#### FilterListingArgs
Параметры фильтрации для Record

From и To параметры выбора временного периоды. From позже чем To, и вообще список всегда упорядочен по EndDate по убыванию даты

- From - время откуда начинать счтитать фильтрует EndTime
- To - время до куда начинать считать, фильтрует EndTime
- Projects - список проектов, по умолчанию null - любой
- Count - количество Record, по умолчанию null
- Date - фильтрация по подходяще дате, может быть null
- IsActive - если IsActive, то To и From должны быть null, может быть null

Метод IsValid(Record) - проверяет подходит ли Record под заданные параметры фильтра

## Инфраструктурные решения
### DI-контейнер
Используется расширение Microsoft.Extensions.DependencyInjection. Регистрируются серивисы в ServiceCollection и создается ServiceProvider. 

Все объекты хранятся внутри контейнера, если нужны параметры для инициализации создаем фабричные методы и инъектируем их в энужный экземпляр

Фабричные методы хранятся в App

#### Список объектов DI-контейнера

- MainWindow via MainViewModel

ViewModels:
- TimerViewModel
- NoteListingViewModel
- TimerLayoutViewModel

Stores:

- ModalStore
- NavigationStore
- TodayDateStore

Services:

- OpenModalNavigationService
- CloseModalNavigationService

Validators:

- NoteValidator

Repository
- TimaNoteRepository

Factories
- Func<Type, ViewModelBase> - factory via ServiceProvider
- NoteFactory

![Диаграмма DI-контейнера](./class_diagrams/images/di-container.svg)

#### Фабричные методы в классе App
- TimeFormFactory
- ModalParameterizedNavigationService - навигация внутри модального окна в качестве параметра принимает ViewModel
- ModalNavigationServiceFactory - сервис навигации ViewModel берется из ServiceProvider. Не используется по умолчанию
- TimerLayoutNavigationServiceFactory<TViewModel> - сервис навигации для TimerLayoutViewModel, исопльзует ContentViewModel типа TViewModel из ServiceProvider. Не используется по умолчанию
- NavigationServiceFactory
- TimerViewModelFactory - создан из-за специфического сервиса навигации

### Навигация
MainViewModel хранит ссылку на NavigationStore и отслеживает изменения в его CurrentViewModel. Каждый конкретный экземпляр INavigationService должен менять INavigationStore.CurrentViewModel под нужные ViewModel

![Алгоритм навигации](./activities/images/navigation.svg)


#### Список переходов в приложении
- При запуске должен открываться TimerLayoutViewModel с контентом NoteListingViewModel
- EditableNote открывает модальное окно TimeFormViewModel
- EditableNote открывает модальное окно ProjectFormViewModel

### Хранение данных