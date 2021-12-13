using Antlr4.StringTemplate;
using System;
using System.Windows.Forms;

namespace TranslatorExplorer
{
    internal class RichTextTemplate
    {
        private RichTextBox Owner;
        private Template Template;
        private string PreTag, PostTag;

        private const string ContentFiller = "<pre_tag><content><post_tag>";

        public RichTextTemplate(RichTextBox owner, string preTag = "", string postTag = "")
        {
            Owner = owner;
            PreTag = preTag;
            PostTag = postTag;
            RecordInitialRTSTate();
        }

        private void RecordInitialRTSTate()
        {
            if (Owner == null || !string.IsNullOrEmpty(Owner.Text))
            {
                throw new InvalidOperationException("TextBox is not empty");
            }

            string rtf_base = Owner.Rtf;
            int ending = rtf_base?.LastIndexOf('}') ?? -1;
            if (ending == -1)
            {
                throw new ArgumentException("Invalid RTF Format");
            }

            for (--ending; char.IsWhiteSpace(rtf_base[ending]); --ending) ;

            string template_str = rtf_base.Insert(ending + 1, ContentFiller);

            Template = new Template(template_str);
            Template.Add("pre_tag", PreTag);
            Template.Add("post_tag", PostTag);
        }

        public string Render(string Content)
        {
            Content.Replace("∧", "\\∧");
            Content.Replace("∨", "\\∨");
            Template.Add("content", Content);
            return Template.Render();
        }
    }
}
