﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtomeJanai.Shared.Common.BmFont.FontModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OtomeJanai.Shared.Common.BmFont
{
    internal class FontRenderer
    {
        private Dictionary<char, FontChar> _characterMap;
        private FontFile _fontFile;
        private List<Texture2D> _textures;
        public FontRenderer(string fontName, GraphicsDevice device)
        {
            //TODO: set the font.7z file "Copy Always" on android and windows 10 platform
            using (var stream = ContentLoader.GetFileStream($"Font/{fontName}.fnt"))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
                _fontFile = (FontFile)deserializer.Deserialize(stream);
            }
            _characterMap = new Dictionary<char, FontChar>();
            _textures = new List<Texture2D>();
            foreach (var fontCharacter in _fontFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                //sometime will already contain in the dictionary and I have no idea why
                if (!_characterMap.ContainsKey(c))
                    _characterMap.Add(c, fontCharacter);
            }
            foreach (var page in _fontFile.Pages)
            {
                using (var stream = new MemoryStream())
                {
                    using (var fstream = ContentLoader.GetFileStream($"Font/{page.File}"))
                        fstream.CopyTo(stream);
                    _textures.Add(Texture2D.FromStream(device, stream));
                }
            }
        }

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text)
        {
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(_textures[fc.Page], position, sourceRectangle, Color.White);
                    dx += fc.XAdvance;
                }
            }
        }



    }

}