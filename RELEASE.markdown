Reworking the plugin infrastructure to allow for faster editing and performance.

# Editor

- Removed the margins from the default editor because it was slowing down the system with large documents. +Changed
- Fixed a bug where the project line buffer was not disconnecting from events properly. +Changed
- Context menu doesn't have Copy, Cut, and Paste (and no automatic separator). +Changed
- Context menu automatically selects first item in menu. +Changed

# Blocks

- Changing text spans through block analysis will cause the line to update on screen. +New
- Properties for blocks and projects are persisted across savings. +New

# Plugins

- Reworking the block analyzers so they can be selectively re-run against the blocks in a collection. +New
- Block analysis is automatically performed when a document is loaded. +New
	- Analysis state is serialized so blocks aren't reanalyzed on loading. +New
- Local Word Spelling:
	- Updated the Local Words spelling plugin to update the entire document when words are added to either dictionaries. +New
	- Text spans are not updated if the spelling errors haven't changed. +New

# Dependencies

- MfGames.GtkExt 0.2.0
- MfGames.GtkExt.TextEditor 0.3.0