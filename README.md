# GranTorismo
Proyecto bases 2

# MongoDB
COMANDOS IMPORTANTES MONGO

--- Iniciar la base desde el cmd

"C:\Program Files\MongoDB\Server\3.4\bin\mongod.exe"

--- Comandos para usar MongoDB en Python

import pymongo

from pymongo import MongoClient

client = MongoClient()

client = MongoClient('localhost', 27017)
