using System.ComponentModel;
using WPFCommandAggregator;

namespace MarcArmbruster.Wpf
{
    public class VmMain : BaseVm, IDataErrorInfo
    {
        protected override void InitCommands()
        {            
        }


        private string inputText;
        public string InputText
        {
            get => this.inputText;
            set => this.SetPropertyValue(ref this.inputText, value);
        }

        public string Error => string.Empty;

        public string this[string propName]
        {
            get
            {
                if (propName == nameof(this.InputText))
                {
                    return this.InputText?.Length > 5 ? "Text is too long!" : string.Empty;
                }

                return string.Empty;
            }
        }
    }
}
