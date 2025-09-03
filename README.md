# repository_manager_lib

Implementation of a a flexible, extensible repository manager library built in C#/.NET, designed to store and retrieve structured content (JSON, XML, and more in the future) with support for pluggable storage backends (in-memory, PostgreSQL, etc.) and validation.

## Features
`Store & retrieve items with unique names`
`Content validation (JSON / XML)`
`Pluggable storage backends:`
- In-memory (thread-safe ConcurrentDictionary)
- PostgreSQL (Npgsql driver)
`Prevents accidental overwrite of registered items`
`Thread-safe design for multi-threaded environments`
`Extensible: easily add new validators (CSV, YAML, etc.) or storage providers (file system, Redis, etc.)`
`Unit tested with xUnit`
