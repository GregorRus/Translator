# Translator
Учебные проект по разработк транслятора.

## Структура проекта
* Translator - Интерфейс командной строки транслятора.
  - [Program.cs](Translator/Program.cs) - Запуск транслятора и передача текстового файла на вход.
* TranslatorExplorer - Оконный интерфейс визуализации работы алгоритмов транслятора.
  - [Form1.cs](TranslatorExplorer/Form1.cs) - Чтение данных из формы окна и передача транслятору, с последующим выводом результатов.
* TranslatorLib - Основные компоненты транслятора.
  - [Transliterator.cs](TranslatorLib/Transliterator.cs) - Транслитератор.
  - [Lexer.cs](TranslatorLib/Lexer.cs) - Лексический анализатор.
  - [SyntaxAnalyzer.cs](TranslatorLib/SyntaxAnalyzer.cs) - Синтаксический анализатор.
  - [HashTable.cs](TranslatorLib/HashTable.cs) - Хэш-таблицы.
  - [Stage.cs](TranslatorLib/Stage.cs) - Вспомогательные интерфейсы для компонетов-этапов.
  - [TranslatorExceptions.cs](TranslatorLib/TranslatorExceptions.cs) - Вспомогательные классы-исключения для ошибок трансляции.

