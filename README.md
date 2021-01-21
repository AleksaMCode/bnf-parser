# BNF parser
<p align="justify"><i>Formal Methods in Software Engineering</i> course project, as taught at the Faculty of Electrical Engineering Banja Luka. I did this project two years ago, but I never got around to opensourcing it.</p>

## Description
<p align="justify">Program reads the grammar definition expressed in BNF (<a href="https://en.wikipedia.org/wiki/Backus%E2%80%93Naur_form">Backus-Naur Form</a>) at runtime after which it parses inputed strings.</p>

## Course project details
<p align="justify">The task was to create a text parser that would parse the contents of the input file, as specified by the grammar in the <i>modified</i> Backus-Naur form. The grammar is given in a configuration file.<br><br>

The modified BNF form is written based on the rules for the standard BNF form, with the following appendices: </p>

- `<a> ::= regex(regular_expression)` <p align="justify"> Denotes a node that is determined on the basis of a regular expression specified in parentheses. In a given case, `<a>` represents any expression that matches the given regular</p>
- `<a> ::= standard_expression` <p align="justify"> Denotes a node that is determined based on tables of standard regular expressions, where in place of standard_expression can be any of the items from the following tables. </p>

Standard_expression | Meaning 
---|---
phone_number | Phone number with or without international and country call codes, with different delimiters
mail_address | Properly formatted e-mail address
web_link | Properly formatted absolute [URL](https://en.wikipedia.org/wiki/URL)
number_constant | Integer or floating-point constant
big_city | Any of the big European cities (first 200)
<br>

### Implementation specifications