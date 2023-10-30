Приложение для сохранения временных промежутков времени (records) с использованием встроенного таймера.

## Use cases

Пользователь может запустить таймер с введенным заголовком и отсчетом от текущего времени или значениями по умолчанию

- Запуск таймера
- Создание record

Во время работы таймера пользователь может менять заголовок, проект и время (время окончания не доступно)

- Изменение Title
- Изменение StartTime
- Изменение Project

Во время остановки таймера Record сохраняется в списке
- Остановка таймера и запись времени окончания в Record

Список сгруппирован по Date, пользователь может создавать, удалять записи, изменять заголовок, проект, время, добавлять заметки
- Удаление Record
- Создание Record без таймера
- Изменение StartTime и EndTime, а также связанного свойства Time

К завершенной Record пользователь может добавлять Заметки Note, заметок может быть неограниченное количество

- Добавление Notes к записям

Пользователь может добавлять проект из списка или создать новый

- Создание проекта

Список группируется по Date

### Дополнительные функции
Список Records, отфильтрованные по проекту

При создании Records в таймере, должны всплываться подсказки содержащие уже созданные records. Подсказки должны фильтроваться должны образом

Импорт данных из toggl

Автозапуск приложения

Система уведомлений

Настройки

![Use Case Diagram](./use-cases/timaproject-use-case.svg)
## Модель данных
Record
- StartTime - время запуска
- EndTime - время остановки, может быть null
- IsDone - завершенность, завершено если есть EndTime
- Title - заголовок
- Notes - список заметок
- Date - дата, связанная с записью
- Project - проект
- Id

> Использование Date
Поле Date представляет собой дату с которая ассоциирована TimaNote. Она нужна для того чтобы ассоциировать записи после полуночи (когда фактически наступил новый день) с предыдущим днем. 

Project
- Name
- Id

Note 
- Text
- Id

### Компоненты

Timer

TimeForm

ProjectForm

ListingRecord

EditableRecord

![Диаграмма классов](./class_diagrams/images/class.svg)

## Алгоритмизация use case и тестирование

### Таймер

#### Запуск таймера
Тесты:
- При запуске таймера создается Record и добавляется в репозиторий
- Record должен иметь StartTime - из поля StartTime, а EndTime - null
- Record должен принимать Title из формы Title или принимать значение по умолчанию
- Record принимает Project из поля Project
- Если поле не прошло валидацию должно ставиться значение по умолчанию
    - Title - пустая строка
    - Project - пустой проект
    - StartTime - текущее время
    - Date - текущая дата

#### Валидация ввода таймера
Тесты:
- Изменение поле Record (Title, Project, StartTime) должно приводить к изменению Record в репозитории
- Если валидация полей не прошла изменения не должны применяться в репозитории
- Если валидация полей не прошла в Timer должна быть ошибка для соответствующего поля
- Если валидация полей прошла, то соответствующие изменения сохраняются в репозитории


### Валидация RecordViewModel
RecordViewModel содержит поля
- StartTime
- EndTime
- Title
- Date

А также логику валидации этих полей с помощь RecordValidator. В случае ошибки валидации NorifyDataError покажет об ошибке поля, при введенные данные сохранятся в поле

Тесты
- Если ввод EndTime приводит к тому что EndTime раньше StartTime это приводит к ошибки по полю EndTIme
- Если ввод StartTime приводит к тому что EndTime раньше StartTime это приводит к ошибки по полю StartTime


#### RecordViewModelWithEdit
Расширение RecordViewModel с формами TimeForm и ProjectForm. Любые корректные изменения из этих форм применяются к полям RecordViewModelWithEdit. Функциональность RecordViewModelWithEdit наследуют TimerViewModel и EditableRecordViewModel

Тесты
- Корретные ввод из TimeFrom (поля StartTime, EndTime, Date) должны применятся к полям RecordWithEdit
- Некорректный ввод из TimeFrom (поля StartTime, EndTime, Date) должен игнорироваться
- Ввод из ProjectForm должен применяться к полю Project, когда там что-то меняется
- Поле TimeForm - при инициализации null
- При инициализации поля TimeFrom, RecordViewModelWithEdit должен подписываться на события TimeForm.PropertyChanged и TimeForm.Closed 

##### TimeForm
Алгоритм использования формы валидации времени у record:
![Алгоритм формы валидации времени](./activities/images/Алгоритм_формы_валидации_времени.png)

При клике на поле должна выходить форма. Форма содержит четыре поля Time, StartTime, EndTime, Date. Time является разностью времени EndTime и StartTime

Изменение Time изменияет StartTime относительно введенного времени. 

Поля StartTime и EndTime могут изменятся по времени и по дате. При этом EndTime не может быть раньше StartTime

Тесты:
- Неправильный ввод StartTime, EndTime, Date, Time должен приводить к ошибки по данному полю
- Если форма редактирует активный Record, то EndTime устанавливается на время открытия формы и недоступно для редактирования
- Если EndTime заблокировано, то оно имеет значение времени установки блокировки
- Если EndTime заблокировано, то оно не меняет свои значения пока IsEndTimeEnebled = false
- Корректное время Time меняет StartTime относительно EndTime
- Если Time - коректное, но EndTime не корректно добавляет ошибку в Time
- Изменение StartTime (корректное) меняет Time
- Изменение EndTime (корректное) меняет Time


##### ProjectForm
Record может не иметь проекта или иметь из списка существующих.
Список проектов также имеет форму для создания нового

Алгоритм изменения проекта
![Алгоритм изменения проекта](./activities/images/Алгоритм_изменения_проекта.png)

##### EditableRecordViewModel
RecordViewModel, который хранит завершенную Record и при любых изменениях отправляет эти изменения в репозиторий

Имеет возможность добавлять заметки к записям

Тесты
- Корректные изменения полей должны изменять Record в репозитории
- Некорректные изменения полей не должны измениять Record в репозитории


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