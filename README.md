# Turing-Assembler

## Synopsis

At the top of the file there should be a short introduction and/ or overview that explains what the project is.
This description should match descriptions added for package managers (Gemspec, package.json, etc.)

An assembly language is a low-level programming language for programming computational devices. A low-level programming language is
one that has a tight correspondence with the hardware of the machine. In this project I developed an assembly language for
Turing Machines. 

The input of the program is a TM (Turing Machine) source file, which contains the initialization and algorithm for the 
Turing machine. Then the program will compile the instructions and will take in a word and test it for acceptance with 
respect to the TM defined in the source file. As the program executes the turing machine, the complete trace of the machine
will be displayed. 
## Code Example

Show what the library does as concisely as possible, developers should be able to figure out how your project 
solves their problem by looking at the code example. Make sure the API you are showing off is obvious, and
that your code is short and concise.

The following is an example of a TM File:

****************************************************************************
-- Initialization: 
{states: Q0,Q1,Q2,Q3,Q4,Q5,Q6,Q7,A,R}
{start: Q0}
{accept: A}
{reject: R}
{alpha: 0,1,#}
{tape-alpha: 0,1,#,x}
-- Main Algorithm:
--   0:
rwRt Q0 0 x Q1;               -- Read, Write, Right, Transition.
rRl Q1 0;                     -- Read, Right, Loop.
rRl Q1 1;
rRt Q1 # Q3;                  -- Read, Right, transition.
rRl Q3 x;
rwLt Q3 0 x Q5;               -- Read, Write, Left, Transition.
--   1:
rwRt Q0 1 x Q2;
rRl Q2 0;                     -- Read, Right, Loop.
rRl Q2 1;
rRt Q2 # Q4;                  -- Read, Right, transition.
rRl Q4 x;
rwLt Q4 1 x Q5;               -- Read, Write, Left, Transition.
--  Find #:
rLl Q5 x;                     -- Read, Left, Loop.
rLt Q5 # Q6;                  -- Read, Left, Transition.
--   Reset:
rLl Q6 0;
rLl Q6 1;
rRt Q6 x Q0;
--   Accept:
rRt Q0 # Q7;
rRl Q7 x;
rRt Q7 _ A;

****************************************************************************

Comments start with a -- and the initialization. The first part of each TM source file is the initialization of the 
hardware which is a list of configurations. Then the rest of the file is a series of commands that defines the algorithm 
of the Turing Machine. The previous exapmle is the definition of the TM that accepst the language: L = {w#w | w ∈ {0, 1}*}.

For futher and more complete instructions look at the TM_Project_Problem.pdf.

## Motivation

This project was created with great enthusiasm for Dr. Harley Eades' Theory of Computation class at Augusta University. 

## Installation

Right now, the application is in raw code and will need to be opened, compiled and run using Visual Studio. I will 
be adding an executable program shortly. 


## Tests

An example of the input and trace of the above TM is located in the TM_Project_Problem.pdf. 


## Contributors

Special thanks to Dr. Harley Eades. 


