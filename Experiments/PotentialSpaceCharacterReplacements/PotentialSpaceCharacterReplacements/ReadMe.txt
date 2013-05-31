This experiments tried to identify characters that could be used as a replacement for the space character.

The following characters were identified:
0559
0951
0952
0A51
0A75
115F
1160
135F
180B
180C
180D
200B
200C
200D
200E
200F
202A
202B
202C
202D
202E
2060
2061
2062
2063
2064
206A
206B
206C
206D
206E
206F
2D6F
3164
FE70
FE72
FE76
FE78
FE7C
FE7E
FEFF
FFA0
FFF9
FFFA
FFFB

Just in case we need them in the code later on:
                return new[]
                           {
                               '\u0559', '\u0951', '\u0952', '\u0A51', '\u0A75', '\u115F', '\u1160', '\u135F', '\u180B',
                               '\u180C', '\u180D', '\u200B', '\u200C', '\u200D', '\u200E', '\u200F', '\u202A', '\u202B',
                               '\u202C', '\u202D', '\u202E', '\u2060', '\u2061', '\u2062', '\u2063', '\u2064', '\u206A',
                               '\u206B', '\u206C', '\u206D', '\u206E', '\u206F', '\u2D6F', '\u3164', '\uFE70', '\uFE72',
                               '\uFE76', '\uFE78', '\uFE7C', '\uFE7E', '\uFEFF', '\uFFA0', '\uFFF9', '\uFFFA', '\uFFFB'
                           }

Only the following four can actually be used like space character replacements:

'\u115F'	"Hangul choseong filler"
'\u1160'	"Hangul jungseong filler"
'\u3164'	"Hangul filler"
'\uFFA0'	"Halfwidth hangul filler"