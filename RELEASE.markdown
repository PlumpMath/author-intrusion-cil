Incremental release to handle bug fixed in the process of using Author Intrusion to write a novella.

# Spelling

- Modified the spelling framework plugins to return an enum-based result of correct, incorrect, or indeterminate. This will allow plugins (NHunspell) to disable themselves if they cannot be loaded. +Changed
- Expanded the logic for the Hunspell plugin to allow for loading through different mechanisms on Linux and Windows. +Changed