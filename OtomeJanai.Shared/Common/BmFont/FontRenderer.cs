using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OtomeJanai.Shared.Common.BmFont.FontModel;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace OtomeJanai.Shared.Common.BmFont
{
    internal class FontRenderer
    {
        private Dictionary<char, FontChar> _characterMap;
        private FontFile _fontFile;
        private List<Texture2D> _textures;
        private bool _isInit;
        public FontRenderer(string fontName, GraphicsDevice device)
        {
            Init(fontName, device);
        }

        private async void Init(string fontName, GraphicsDevice device)
        {
            using (var stream = await ContentLoader.GetFileStream($"Font/{fontName}.fnt"))
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
                using (var stream = await ContentLoader.GetFileStream($"Font/{page.File}"))
                using (var fstream = new MemoryStream())
                {
                    await stream.CopyToAsync(fstream);
                    //On android, fstream.Position isn't 0
                    fstream.Position = 0;
                    _textures.Add(Texture2D.FromStream(device, fstream));
                }
            }
            _isInit = true;
        }

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, Color color)
        {
            if (!_isInit)
                return;
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                FontChar fc;
                if (!_characterMap.TryGetValue(c, out fc))
                    continue;
                //TODO: Text Wrapping
                //TODO: DPI
                var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);
                spriteBatch.Draw(_textures[fc.Page], position, sourceRectangle, color);
                dx += fc.XAdvance;
            }
        }

        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text)
            => DrawText(spriteBatch, x, y, text, Color.White);
    }

}
