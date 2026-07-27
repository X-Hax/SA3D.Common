# SA3D.Common
Code reused all throughout the SA3D Code Library.

---
## Namespaces and their contents

| Namespace              		| Contents                                                                            	|
|------------------------		|--------------------------------------------------------------------------------------	|
| SA3D.Common            		| Various Utility Methods                                                              	|
| SA3D.Ascii					| Writer for writing Ascii C-structures													|
| SA3D.Common.Lookup     		| 2-way pointer dictionaries for storing pairs with unique values and unique addresses 	|
| SA3D.Common.Ini        		| Ini data de/serializer.                                                              	|
| SA3D.Common.IO         		| Data reader and writer, as well as Executable utilities.                             	|
| SA3D.Common.Converters 		| Various converters, mostly used in conjunction with Ini data.                        	|
| SA3D.Common.JsonConverters	| Various Json converters for native data types.			                        	|

---

## Utility classes

| Class                	| Function                                                                               	|
|----------------------	|----------------------------------------------------------------------------------------	|
| FlagHelper           	| Flag values from 8 to 64 bits.                                                         	|
| CollectionExtensions 	| Collection utility methods.                                                            	|
| MathHelper           	| Various mathematical functionalities. Primarily contains angle related methods so far. 	|
| RegionMarker         	| Used to define and check "regions" in a 1 dimensional sequence.                        	|
| StringExtensions     	| Various extensions for formatting and generating strings.                              	|

## Releasing
!! Requires authorization via the X-Hax organisation

1. Edit the version number in src/SA3D.Common/SA3D.Common.csproj; Example: `<Version>1.0.0</Version>` -> `<Version>2.0.0</Version>`
2. Commit the change but dont yet push.
3. Tag the commit: `git tag -a [version number] HEAD -m "Release version [version number]"`
4. Push with tags: `git push --follow-tags`

This will automatically start the Github `Build and Publish` workflow
