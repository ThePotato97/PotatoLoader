// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license. 
// See license.txt file in the project root for full license information.
using PotatoLoader.Tomlyn.Collections;

namespace PotatoLoader.Tomlyn.Text
{
    /// <summary>
    /// (trait) CharacterIterator ala Stark
    /// </summary>
    // ReSharper disable once InconsistentNaming
    internal interface CharacterIterator : Iterator<char32, int>
    {
    }
}