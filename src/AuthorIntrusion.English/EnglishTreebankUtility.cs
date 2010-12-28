#region Copyright and License

// Copyright (c) 2005-2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;

using AuthorIntrusion.English.Enumerations;

#endregion

namespace AuthorIntrusion.English
{
	/// <summary>
	/// Utility class for working with treebank codes.
	/// </summary>
	public static class EnglishTreebankUtility
	{
		/// <summary>
		/// Gets the classification of a treebank code.
		/// </summary>
		/// <param name="treebankCode">The treebank code.</param>
		/// <returns></returns>
		public static EnglishTreebankClassification GetClassification(
			string treebankCode)
		{
			// Make sure we have valid input.
			if (String.IsNullOrEmpty(treebankCode))
			{
				throw new ArgumentNullException("treebankCode");
			}

			// Use the code in a switch statement to identify the type.
			switch (treebankCode)
			{
				case "#":
				case "(":
				case ")":
				case "$":
				case ",":
				case ":":
				case "\"":
				case "'":
					return EnglishTreebankClassification.Puncuation;

				case ".":
					return EnglishTreebankClassification.Terminating;

				case "ADJP":
				case "ADVP":
				case "NP":
				case "VP":
				case "PP":
				case "S":
				case "SBAR":
				case "SBARQ":
				case "SINV":
				case "SQ":
				case "WHADVP":
				case "WHNP":
				case "WHPP":
				case "X":
					return EnglishTreebankClassification.Phrase;

				default:
					return EnglishTreebankClassification.Word;
			}
		}
	}
}

/*
http://www.ldc.upenn.edu/Catalog/docs/LDC95T7/cl93.html
 
1. CC  Coordinating conjunction  25.TO  to 
2. CD  Cardinal number           26.UH  Interjection 
3. DT  Determiner                27.VB  Verb, base form 
4. EX  Existential there  	 28.VBD Verb, past tense 
5. FW  Foreign word              29.VBG Verb, gerund/present participle 
6. IN  Preposition/subord.  	 30.VBN Verb, past participle 
218z     conjunction 
7. JJ  Adjective                 31.VBP Verb, non-3rd ps. sing. present 
8. JJR Adjective, comparative    32.VBZ Verb, 3rd ps. sing. present 
9. JJS Adjective, superlative    33.WDT wh-determiner 
10.LS  List item marker          34.WP  wh-pronoun 
11.MD  Modal                     35.WP  Possessive wh-pronoun 
12.NN  Noun, singular or mass    36.WRB wh-adverb 
13.NNS Noun, plural              37. #  Pound sign 
14.NNP Proper noun, singular     38. $  Dollar sign 
15.NNPS Proper noun, plural      39. .  Sentence-final punctuation 
16.PDT Predeterminer             40. ,  Comma 
17.POS Possessive ending         41. :  Colon, semi-colon 
18.PRP Personal pronoun          42. (  Left bracket character 
19.PP  Possessive pronoun        43. )  Right bracket character 
20.RB  Adverb                    44. "  Straight double quote 
21.RBR Adverb, comparative       45. `  Left open single quote 
22.RBS Adverb, superlative       46. "  Left open double quote 
23.RP  Particle                  47. '  Right close single quote 
24.SYM Symbol 			 48. "  Right close double quote
 
1.  ADJP     Adjective phrase 
2.  ADVP     Adverb phrase 
3.  NP       Noun phrase 
4.  PP       Prepositional phrase 
5.  S        Simple declarative clause 
6.  SBAR     Clause introduced by subordinating conjunction or
             0 (see below) 
7.  SBARQ    Direct question introduced by wh-word or wh-phrase 
8.  SINV     Declarative sentence with subject-aux inversion 
9.  SQ       Subconstituent of SBARQ excluding wh-word or wh-phrase 
10. VP       Verb phrase 
11. WHADVP   Wh-adverb phrase 
12. WHNP     Wh-noun phrase 
13. WHPP     Wh-prepositional phrase 
14. X        Constituent of unknown or uncertain category 

*/