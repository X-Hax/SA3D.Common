# SA3D.Common
Code reused all throughout the SA3D Code Library.

---
## Namespaces and their contents

| Namespace              	| Contents                                                                            	|
|------------------------	|--------------------------------------------------------------------------------------	|
| SA3D.Common            	| Various Utility Methods                                                              	|
| SA3D.Common.Lookup     	| 2-way pointer dictionaries for storing pairs with unique values and unique addresses 	|
| SA3D.Common.Ini        	| Ini data de/serializer.                                                              	|
| SA3D.Common.IO         	| Data reader and writer, as well as Executable utilities.                             	|
| SA3D.Common.Converters 	| Various converters, mostly used in conjunction with Ini data.                        	|

---

## Utility classes

| Class                	| Function                                                                               	|
|----------------------	|----------------------------------------------------------------------------------------	|
| DistinctMap<T>       	| For calculating and storing distinct values and an index mapping.                      	|
| FlagHelper           	| Flag values from 8 to 64 bits.                                                         	|
| CollectionExtensions 	| Collection utility methods.                                                            	|
| MathHelper           	| Various mathematical functionalities. Primarily contains angle related methods so far. 	|
| RegionMarker         	| Used to define and check "regions" in a 1 dimensional sequence.                        	|
| StringExtensions     	| Various extensions for formatting and generating strings.                              	|
