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

// 
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

Конфигурация компонента доступна через метод `Configure` или через конструктор класса `CommandParser`.

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

## Точки расширения
`TODO`
