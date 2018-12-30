// render katex math
//import showdownKatex from 'showdown-katex/src/showdown-katex'

// render code highlight


$(document).ready(function ()
{
	$('.markdown').hide();
	renderMathInElement
	(
		document.body,
		{
			delimiters:
				[
				{left: "$[", right: "]$", display: true},
				{left: "\\[", right: "\\]", display: true},
//				{left: "$$(", right: ")$$", display: false},
				{left: "$(", right: ")$", display: false},
				{left: "\\(", right: "\\)", display: false}
			]
		}
	);
	// showdown.extension('showdownKatex', showdownKatex);
	const converter = new showdown.Converter({
		// extensions: [
		// 	showdownKatex({
		// 		// maybe you want katex to throwOnError
		// 		throwOnError: true,
		// 		// disable displayMode
		// 		displayMode: false,
		// 		// change errorColor to blue
		// 		errorColor: '#1500ff',
		// 	}),
		// ],
		headerLevelStart: 3
	});
//	converter.makeHtml('~x=2~');
	
	// var converter = new showdown.Converter({headerLevelStart: 3});
	$('.markdown').each(function ()
	{
		$(this).html(converter.makeHtml($(this).html()));
		$(this).html($(this).html().replace(/&amp;/g, '&'));
		console.log($(this).html());
//		block.innerHTML = converter.makeHtml(block.innerHTML);
//		block.innerHTML = block.innerHTML.replace(/&amp;/g, '&');
//		block.innerHTML = block.innerHTML.replace(/&lt;/g, '<');
//		block.innerHTML = block.innerHTML.replace(/&gt;/g, '>');
	});
	
	$('.markdown').show();
	
	
	$('.language-c, .language-cpp, .language-java, .language-python, .language-cs').each(function (i, block)
	{
		hljs.highlightBlock(block);
	});
});
