---
title: Notes
---

# JSON

Parsing a JSON.NET string:

	JObject.Parse(jsonString).SelectToken("hashKey").ToObject<TType>();

# Tokens

* Retokenize a line
* Handle deleting at the end of the buffer
	* Raise a operation at end of buffer?
* Handle delete left
	* At the beginning of the buffer
	* In the middle of a line
	* At the beginning of a line
* Immediate substitutions
* Deferred processing
* Styling tokens
	* Deferred styling of tokens
