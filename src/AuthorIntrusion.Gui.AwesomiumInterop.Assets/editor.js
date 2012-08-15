paragraphHtml = {};
paragraphId = 1;
hasParagraphChanges = false;

function test()
{
	var previous = 'This is <span style="color: blue;">paragraph</span> 2<span id="selectionBoundary_234_234324" style="line-height: 0; display: none;" class="rangySelectionBoundary"></span>';

	var input = 'This is <span style="color: blue;">paragraph</span> 2';
	
	// Figure out where the selection starts as characters. The changed paragraphs will never add characters if
	// we are updating the current selection.
	var characters = 0, start = -1, stop = -1, last = -1, inTag = false;
	
	for (var i = 0; i < previous.length; i++)
	{
		c = previous[i];
		
		if (c === '<')
		{
			// Mark that we are inside a tag.
			inTag = true;
			last = i;
			
			if (start < 0) start = i;
		}
		else if (!inTag)
		{
			characters++;
		}
		else if (inTag && c === '>')
		{
			// Mark that we are no longer in a tag.
			inTag = false;
			
			// See if we have the selection we are looking for.
			if (stop < 0)
			{
				var tag = previous.substring(start, i + 1);
			
				if (tag.indexOf("selectionBoundary") >= 0)
				{
					stop = i + 1;
					alert(previous.substring(start, stop));
				}
				else
				{
					// Reset the stop since this isn't the tag.
					start = -1;
				}
			}
		}
	}
	
	alert(previous + "\n\n" + "ch: " + characters + ", start=" + start + ", stop=" + stop + "\n\n" + input);
}

function resetEditorParagraphs()
{
	// Empty out the contents so we can detect new changes.
	paragraphHtml = {};

	// Goes through all the paragraphs and saves the HTML contents so
	// we can detect changes to the paragraph.
	$("[contenteditable] p").each(function() {
		paragraphHtml[this.id] = $(this).html();
	});
}

function reindexEditorParagraphs()
{
	// Keep track of the IDs we've already seen.
	seenIds = {}
	seenIds[undefined] = 0;

	$("[contenteditable] p").each(function() {
		// Pull out the ID for the paragraph.
		id = $(this).attr('id') || undefined;

		// Check to see if we have seen this ID already.
		if (id in seenIds)
		{
			// We have a duplicate or missing ID so assign a new one.
			newId = "p-" + paragraphId;
			paragraphId += 1;

			// Assign the ID.
			$(this).attr('id', newId);

			// Change the style to the dirty one.
			$(this).removeClass('normal');
			$(this).removeClass('pending');
			$(this).addClass('dirty');

			// Put a "bad" value into the change hash.
			paragraphHtml[newId] = undefined;
		}
		else
		{
			// Check to see if this paragraph changes.
			if (paragraphHtml[id] !== $(this).html())
			{
				$(this).removeClass('normal');
				$(this).removeClass('pending');
				$(this).addClass('dirty');
			}
		}

		// Keep track that we've seen this ID.
		seenIds[id] = 0;
	});
}

function setEditorParagraph(id, html)
{
	// Update the cached HTML version so we can detect changes.
	paragraphHtml[id] = html;
	
	// Save the selection so we can restore it.
	var selection = rangy.saveSelection();
	
	alert($("[contenteditable] p#" + id).html() + "\n\n" + html);
	
	// Update the HTML inside the DOM itself.
	$("[contenteditable] p#" + id)
		.html(html)
		.removeClass("dirty")
		.removeClass("pending")
		.addClass("normal");
		
	// Restore the selection.
	rangy.restoreSelection(selection);
}

function setEditorParagraphIndex(id)
{
	paragraphId = id;
}

function triggerEditorParagraphChanges()
{
	// Mark the flag that we have changes to parse.
	hasParagraphChanges = true;
	
	// If we have the callback item, trigger it.
	if (AuthorIntrusionGuiCallback)
	{
		AuthorIntrusionGuiCallback.TriggerChange();
	}
}

function getEditorParagraphChanges()
{
	// Variables we'll be populating with the loops.
	paragraphs = {};
	deletedIds = [];
	count = 0;
	
	// For performance reasons, we only want to loop through data if we actually have changes.
	if (hasParagraphChanges)
	{
		// Reset the editor changes which is used to pick up new paragraphs. Chrome typically copies
		// all the attributes of the previous paragraph, including "id" fields and we need to be able
		// to identify the paragraphs separately.
		reindexEditorParagraphs();

		// Keep track the current known paragraphs so we can detect deleted lines.
		deletedHtml = jQuery.extend(true, {}, paragraphHtml);

		// Go through all the paragraphs inside the editor.
		$("[contenteditable] p").each(function() {
			// Delete the line from the deletedHtml since we've seen it.
			delete deletedHtml[this.id];

			// Check to see if the line changes.
			currentHtml = $(this).html();
			previous_html = paragraphHtml[this.id];

			if (currentHtml !== previous_html)
			{
				// Create a record for this changed line.
				record = {}
				record['html'] = currentHtml;
				record['previous'] = $(this).prev().attr('id');
				record['next'] = $(this).next().attr('id');
				paragraphs[this.id] = record;

				// Reset the paragraph text so we forget these chanegs.
				paragraphHtml[this.id] = currentHtml;

				// Change the style of this paragraph to pending.
				$(this).removeClass('normal');
				$(this).removeClass('dirty');
				$(this).addClass('pending');
				
				// Increment the counter so we can optimize the output.
				count += 1;
			}
		});

		// Go through the remaining deleted records to indicate they have
		// been deleted.
		for (var id in deletedHtml)
		{
			// Add it to the list of deleted IDs.
			deletedIds.push(id);

			// Remove it from the paragraph cache.
			delete paragraphHtml[id];

			// Increment the counter so we can optimize the output.
			count += 1;
		}
	}
	
	// Clear the changes flag.
	hasParagraphChanges = false;
	
	// Create a container object.
	results = {};
	results.changes = paragraphs;
	results.deleted = deletedIds;

	// Convert the results into a JSON string and return it.
	if (count === 0)
	{
		return "";
	}
	
	return JSON.stringify(results);
}

function displayEditorParagraphChanges()
{
	results = getEditorParagraphChanges();
	$('pre#json').html(results);
}

function loadEditor()
{
	// Reset the current paragraphs which we use to determine if we
	// made a change to any later paragraph.
	resetEditorParagraphs();

	// Reset the paragraph index to the new value.
	setEditorParagraphIndex(100);

	// Hook up the buttons.
	$('button#getEditorParagraphChanges')
		.on('click', displayEditorParagraphChanges);
	$('button#bob')
		.on('click', test);

	// Hook up events on the contenteditable section.
	$('[contenteditable]').live('focus', function() {
		var $this = $(this);
		$this.data('before', $this.html());
		return $this;
	}).live('blur keyup paste', function() {
		var $this = $(this);
		if ($this.data('before') !== $this.html()) {
			$this.data('before', $this.html());
			$this.trigger('change');
		}
		return $this;
	});
	
	// Hoop up the change event to our functionality.
	$('[contenteditable]').bind('change', function() {
		triggerEditorParagraphChanges();
	});
}

// Load the editor and prepare the initial contents.
$(document).ready(loadEditor);