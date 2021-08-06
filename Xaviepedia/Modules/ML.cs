using Discord.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xaviepedia.Modules
{
    public class ML : ModuleBase
    {
        [Command("Predict")]
        public async Task PredictAsync([Remainder]string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("You must provide text for the prediction");
            }

            var filename = RandomFilename() + ".jpg";
            var msg = await ReplyAsync("Your image is generating...");
            Process.Start("python", $"predict.py -T \"{text.Replace("\"", "\\\"")}\" -o " + filename).WaitForExit();
            await Context.Channel.SendFileAsync(filename);
            File.Delete(filename);
            await msg.DeleteAsync();
        }

        [Command("Train")]
        public async Task TrainAsync()
        {
        }

        private string RandomFilename()
        {
            return DateTime.Now.ToString("HHmmssff") + Context.User.Id.ToString();
        }

        private string GetAttachmentImage()
        {
            if (Context.Message.Attachments.Count == 0)
            {
                throw new ArgumentException("You must provide an image");
            }
            return Context.Message.Attachments.ElementAt(0).Url;
        }
    }
}
