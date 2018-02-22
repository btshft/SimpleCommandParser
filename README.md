# Just another simple library 

[![NuGet version (Newtonsoft.Json)](https://img.shields.io/nuget/v/SimpleCommandParser.svg?style=flat-square)](https://www.nuget.org/packages/SimpleCommandParser/)

Ещё одна библиотека, которая начинается с префикса Simple. А как их ещё называть?

## Назначение
Предоставляет механизм для разбора пользовательского ввода формата по типу команды в строготипизированную модель

## Пример

### Разбор одной команды

```C#
class MyCommand {
      [Argument(key: "arg1")]
      public string OptionOne { get; set; }

      [Argument(key: "arg2", required: false)]
      public string OptionTwo { get; set } 
}

CommandParser.Default.ParseCommand<MyCommand>("signal -arg1 abc -arg2 dv")
        .WhenParsed(MyCommand command => DoSomething(command))
        .WhenNotParsed(err => HandleError(err));

```

### Разбор нескольких команд

При использовании дефолтного парсера атрибут `[Verb]` является обязательным. При этом имена команд должны быть уникальными.

```C#
[Verb("one")]
class OneCommand { ... }

[Verb("two")]
class TwoCommand { ... }

CommandParser.Default.ParseCommands("one -arg1 abs -arg2 de", new [] { typeof(OneCommand), typeof(TwoCommand) })
        .WhenParsed<OneCommand>(c1 => HandleOne(c1))
        .WhenParsed<TwoCommand>(c2 => HandleTwo(c2))
        .WhenNotParsed(err => HandleError(err));
```

## Конфигураци

Конфигурация компонента доступна через метод `Configure` или через конструктор класса `CommandParser`.

```C#
CommandParser.Default.Configure(MutableCommandParserSettings settings => UpdateCommandParserSettings(settings));
```

### Настройки

* `StringComparsion` - Культура сравнения строк при разборе команд. Значение по умолчанию: `StringComparison.InvariantCultureIgnoreCase`;
* `CommandVerbPrefix` - Префикс перед глаголом команды. В команде `/signal` префикс это символ `/`. Значение по умолчанию: `null`;
* `CommandArgumentKeyPrefix` - Префикс перед ключом параметра команды. В команде `/signal -arg1 value` префикс это символ `-`. Значением по умолчанию `-`;
* `CommandArgumentKeyValueDelimeter` - Разделитель между ключом и значением параметра команды, например `/signal arg:value`. Параметр является обязательным в случае, если для параметра `CommandArgumentKeyPrefix` задается значение `null`. Значение по умолчанию: `null` (эквивалентно символу пробела).

## Точки расширения
`TODO`
