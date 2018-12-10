
# Stanford CoreNLP along with Probability Matrix NLP

## Running Example of Stanford CoreNLP in C#

CoreNLP(Natural Language Processing) engine was built by Stanford University USA.
Probability Matrix NLP was coded by us.

## Table of Contents
- [Stanford CoreNLP](#stanford-corenlp)
    - [Running Example of Stanford CoreNLP in C](#running-example-of-stanford-corenlp-in-c)
    - [Table of Contents](#table-of-contents)
    - [Overview](#overview)
    - [Development](#development)
    - [Install Dependencies](#install-dependencies)
	-[Outcomes and How to call API](#outcomes-and-how-to-call-api)



## Overview
Stanford CoreNLP’s goal is to make it very easy to apply a bunch of linguistic analysis tools to a piece of text.

https://stanfordnlp.github.io/CoreNLP/

## Development
This project is a modified example of Stanford NLP given in below link.

https://sergey-tihon.github.io/Stanford.NLP.NET/StanfordCoreNLP.html

## Install Dependencies
The following packages are needed to be installed by NuGet Package Manager:

- Common.Logging
- Common.Logging.Core
- IKVM
- slf4j-api
- Stanford.NLP.CoreNLP

Besides, you need the download and extract the follow jar file in your application in correct path

stanford-english-corenlp-2018-10-05-models.jar

## Outcomes and How to call API

https://.....ip..../api/values/query

example:

API call:-

https://localhost:9000/api/values/can you create a task for me

response:-

{
"intent" : "create" ,
"noun" : "task" ,
"action_prob" : "1",
"noun_prob" : "1"
}


