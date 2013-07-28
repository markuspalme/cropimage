using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Widget;

namespace CropImage
{
    [Activity(Label = "CropImage", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private const int PICK_FROM_CAMERA = 1;
        private Android.Net.Uri mImageCaptureUri;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.button);

            button.Click += (s, e) => doTakePhotoAction();

        }

        private Java.IO.File createDirectoryForPictures()
        {
            var dir = new Java.IO.File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures), "Boruto");
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }

            return dir;
        }

        private void doTakePhotoAction()
        {

            Intent intent = new Intent(MediaStore.ActionImageCapture);

            mImageCaptureUri = Android.Net.Uri.FromFile(new Java.IO.File(createDirectoryForPictures(), string.Format("myPhoto_{0}.jpg", System.Guid.NewGuid())));

            intent.PutExtra(MediaStore.ExtraOutput, mImageCaptureUri);

            try
            {
                intent.PutExtra("return-data", false);
                StartActivityForResult(intent, PICK_FROM_CAMERA);
            }
            catch (ActivityNotFoundException e)
            {
                e.PrintStackTrace();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {

            if (resultCode != Result.Ok)
            {
                return;
            }

            switch (requestCode)
            {
                case PICK_FROM_CAMERA:
                    Intent intent = new Intent(this, typeof(CropImage));
                    intent.PutExtra("image-path", mImageCaptureUri.Path);
                    intent.PutExtra("scale", true);
                    StartActivity(intent);
                    break;
            }
        }
    }
}


