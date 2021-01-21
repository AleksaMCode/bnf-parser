# BNF parser
<p align="justify"><i>Formal Methods in Software Engineering</i> course project, as taught at the Faculty of Electrical Engineering Banja Luka. I did this project a year and a half ago, but I never got around to opensourcing it.</p>

## Description
<p align="justify">Program reads the grammar definition expressed in BNF (<a href="https://en.wikipedia.org/wiki/Backus%E2%80%93Naur_form">Backus-Naur Form</a>) at runtime after which it parses inputed strings.</p>

## Course project details
<p align="justify">The task was to create a text parser that would parse the contents of the input file, as specified by the grammar in the <i>modified</i> Backus-Naur form. The grammar is given in a configuration file.<br>

The modified BNF form is written based on the rules for the standard BNF form, with the following appendices:</p>

- `<a> ::= regex(regular_expression)` <p align="justify"> Denotes a node that is determined on the basis of a regular expression specified in parentheses. In a given case, `<a>` represents any expression that matches the given regular</p>
- `<a> ::= standard_expression` <p align="justify"> Denotes a node that is determined based on tables of standard regular expressions, where in place of standard_expression can be any of the items from the following tables.</p>

Standard_expression | Meaning 
---|---
phone_number | Phone number with or without international and country call codes, with different delimiters
mail_address | Properly formatted e-mail address
web_link | Properly formatted absolute [URL](https://en.wikipedia.org/wiki/URL)
number_constant | Integer or floating-point constant
big_city | Any of the big European cities (first 200)

### Implementation specifications
- <p align="justify">The configuration file (config.bnf), must contain in a modified BNF form a specification of the parsing method.</p>
- <p align="justify">As command line argument program should accept input and output files, where the output file can only be <b>XML</b> file.</p>
- <p align="justify"><b>XML</b> tags of the output file must be named based on the token name in the BNF form, so that the resulting parsing tree is clear and equivalent to the specified BNF form</p>
- <p align="justify">In case the input file cannot be parsed according to the BNF form, print a message on the standard output stream about where the error occurred (i.e. the line from the BNF specification on which the error occurred).</p>

## References
<ul>
<li><p align="justify">Dick Grune, Ceriel J. H. Jacobs - <i>Parsing Techniques: A Practical Guide (Monographs in Computer Science)</i></p></li>
<li><p align="justify">Peter Linz - <i>An Introduction to Formal Languages and Automata</i></p></li>
<li><p align="justify">Peter Sestoft, Ken Friis Larsen - <i>Grammars and parsing with C# 2.0</i></p></li>
<li><p align="justify">Anil Maheshwari, Michiel Smid - <i>Introduction to Theory of Computation</i></p></li>