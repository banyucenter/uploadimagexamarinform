using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Net.Http;

namespace UploadServer
{
    public partial class UploadServerPage : ContentPage
    {
        private MediaFile _mediafile;
        

        public UploadServerPage()
        {
            InitializeComponent();
        }


        //Fungsi ambil gambar dari galeri
        private async void Pick_Clicked(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No Pick Photo", ":(No Pick Photo available.", "ok");
                return;
            }

            _mediafile = await CrossMedia.Current.PickPhotoAsync();

            if (_mediafile == null)
                return;

            LocalPathLabel.Text = _mediafile.Path;

            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediafile.GetStream();

            });

        }


        //Fungsi ambil gambar dari camera
        private async void Take_Clicked(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":(No Camera available.", "OK");
                return;

            }

            _mediafile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg"
                });



            if (_mediafile == null)
                    return;

            LocalPathLabel.Text = _mediafile.Path;

            FileImage.Source = ImageSource.FromStream(() =>
             {
                    return _mediafile.GetStream();
              });
                                                                     
          }

        //Fungsi upload ke server CodeIgniter

        private async void Upload_Clicked(object sender, System.EventArgs e)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(_mediafile.GetStream()),
                        "\"file\"",
                        $"\"{_mediafile.Path}\"");

            var client = new HttpClient();
            var uploadServiceBaseAddress = "https://smkmaarifnu1ajibarang.sch.id/RestApi/index.php/api";
            var httpResponseMessage = await client.PostAsync(uploadServiceBaseAddress, content);
            RemotePathLabel.Text = await httpResponseMessage.Content.ReadAsStringAsync();


        }

    }


}

