# Очередная  'Simple' библиотека

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/SimpleCommandParser.svg?style=flat-square)](https://www.nuget.org/packages/SimpleCommandParser/)

Да, это ещё одна библиотека, которая начинается с префикса Simple. А как их ещё называть?

## Назначение
Предоставляет механизм для разбора пользовательского ввода формата по типу команды в строготипизированную модель.

## Пример

### Разбор одной команды

```C#
class CreatePackageCommand
{ 
      [Parameter("n", "name")]
      public string Name { get; set; }
      
      [Parameter("t", "tag", Required = false)]
      public string Tag { get; set; }
      
      [Option("h", "hidden")]
      public bool IsHidden { get; set; }
}

var input = "create :n 'package name' :t cool_package :s :h"; 

/*
var input = "create :name 'package name' :tag cool_package :hidden";
*/

CommandParser.Default.ParseCommand<CreatePackageCommand>(input)
        .WhenParsed(CreatePackageCommand command => DoSomething(command))
        .WhenNotParsed(err => HandleError(err));
```

### Разбор нескольких команд

При использовании дефолтного парсера атрибут `[Verb]` является обязательным. При этом имена команд должны быть уникальными.

```C#
[Verb("create")]
class CreatePackageCommand { ... }

[Verb("update")]
class UpdatePackageCommand { ... }

CommandParser.Default.ParseCommands("update :name 'package' :new_name 'new package'", new [] { typeof(CreatePackageCommand), typeof(UpdatePackageCommand) })
        .WhenParsed<OneCommand>(c1 => HandleOne(c1))
        .WhenParsed<TwoCommand>(c2 => HandleTwo(c2))
        .WhenNotParsed(err => HandleError(err));
```

## Конфигурация

Конфигурация компонента доступна через метод `Configure` или через конструктор класса `CommandParser`. Конфигурацию можно выполнить всего один раз используя метод `Configure` в случае использования глобального экземпляра парсера `CommandParser.Default` и один раз при использовании конструктора без параметров. Последующие попытки вызвать метод конфигурации приведут к исключению. Механизм реализован с целью не допустить различия в поведении одного парсера в разных частях приложения. Узнать о состоянии инициализации парсера можно используя флаг ```CommandParser.IsConfigured```.

```C#
CommandParser.Default.Configure(
      MutableCommandParserSettings settings => ConfigureSettings(settings));
```
```C#
var parser = new CommandParser(
      MutableCommandParserSettings settings => ConfigureSettings(settings));
```

### Настройки

* `StringComparsion` - Культура сравнения строк при разборе команд. Значение по умолчанию: `StringComparison.InvariantCultureIgnoreCase`;
* `VerbPrefix` - Префикс перед действием команды. В команде `/signal` префикс это символ `/`. Значение по умолчанию: `null`;
* `ArgumentKeyPrefix` - Префикс перед ключом параметра команды. В команде `/signal :arg1 value` префикс это символ `:`. Значением по умолчанию `:`;
* `RequireArgumentKeyPrefix` - Указывает на обязательность указания префикса параметра в команде. Отключение позволяет выполнять разбор команд вида `command one two three` при этом значения параметров будут отображены на модель в соответствии со порядком атрибутов `Parameter` на свойствах. Флаги в таком режиме не поддерживаются, как и явное указание отдельных параметров с ключами. Отключать настройку лучше только в случае острой необходимости. Значение по умолчанию `true`.

## Точки расширения
Библиотека позволяет как заменить парсер целиком, реализовав интерфейс `ICommandParser`, так и расширить его функционал, унаследовав свой собственный великолепный парсер от моего скучного `CommandParser`. Однако, если вы правда собираетесь это делать, зачем вы скачали библиотеку? Впрочем, не мне вас судить.


Помимо полной замены парсера можно заменить отдельные его компоненты. Всего для замены предоставляется 3 компонента:
1. Компонент для превращения строки в набор токенов. В библиотеке он представлен интерфейсом `ICommandTokenizer` и стандартной реализацией `DefaultCommandTokenizer`;
2. Компонент для инициализации экземпляра модели команды. Его обязанность перенести полученные токены на свойства экземпляра модели. В библиотеке представлен интерфейсом `ICommandInitializer` и стандартной реализацией `ParameterAttributeBasedCommandInitializer`;
3. Компонент для определения типа модели на основе разобранной на токены команды. Используется при вызове `CommandParser.ParseCommands`. В библиотеке представлен интерфейсом `ICommandTypeResolver` и стандартной реализацией `VerbAttributeBasedCommandTypeResolver`.

Замену компонентов парсера на свои можно произвести используя `CommandParserBuilder`. Пример представлен ниже
``` C#

var builder = new CommandParserBuilder();
var parser = builder.
      .WithTypeResolver(new CustomCommandTypeResolver())
      .WithInitializer(new CustomCommandInitializer())
      .WithTokenizer(new CustomCommandTokenizer())
      .WithSettings(settings => ConfigureSettings(settings))
      .Build();

var command = parser.ParseCommand<Sample>("...");
```
Примеры замены компонентов билдера представлены в файле `Examples.cs` проекта `SimpleCommandParser.Examples`.

> gl hf gh gf
