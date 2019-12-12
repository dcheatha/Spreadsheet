# Excel-like project for CPT_S 321 (Object-Oriented Software Principles)

## Summary
This was the Spreadsheet Application for Object-Oriented Software Principles, a course designed to give students experience creating a large software project. The program supports setting cells equal to one another, adding additional operators, using cells in equations, and has circular reference detection. The expresison tree uses a composite design pattern, and so most equations are supported.

## Features
* Circular reference detection
* Undo & Redo
* Referencing cells in equations
* Cells automatically update when referenced cells are changed
* Variables may be defined and used in equations
* Validation for undefined variables
* Expressions are compiled and only rebuilt when the expression changes
* Expressions support parentheses 
* ExpressionTree uses composite design pattern
* Additional operators may be added
* Standalone Logic engine

## Class Diagram 
![Class Diagram](/SpreadsheetEngineUML.png)
