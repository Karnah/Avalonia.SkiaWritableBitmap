using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;

using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;

namespace Avalonia.SkiaWritableBitmap.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const int FRAMES_COUNT = 26;
        private const int WIDTH = 800;
        private const int HEIGHT = 600;

        private readonly Bitmap[] _unitWriteableBitmaps;
        private readonly Bitmap[] _auraWriteableBitmaps;
        private readonly Bitmap[] _unitResavedBitmaps;
        private readonly Bitmap[] _auraResavedBitmaps;

        private int index = 0;
        private Bitmap _unitWriteableBitmap;
        private Bitmap _auraWriteableBitmap;
        private Bitmap _unitResavedBitmap;
        private Bitmap _auraResavedBitmap;

        public MainWindowViewModel()
        {
            Background = new Bitmap("Resources/Background.png");

            _unitWriteableBitmaps = GetWriteableBitmaps("Resources/UnitImageData/UnitImageData");
            _auraWriteableBitmaps = GetWriteableBitmaps("Resources/AuraImageData/AuraImageData");

            _unitResavedBitmaps = GetResavedBitmap(_unitWriteableBitmaps);
            _auraResavedBitmaps = GetResavedBitmap(_auraWriteableBitmaps);

            var timer = new Timer(75);
            timer.Elapsed += OnUpdate;
            timer.Start();
        }

        private void OnUpdate(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            UnitWriteableBitmap = _unitWriteableBitmaps[index];
            AuraWriteableBitmap = _auraWriteableBitmaps[index];
            UnitResavedBitmap = _unitResavedBitmaps[index];
            AuraResavedBitmap = _auraResavedBitmaps[index];

            ++index;
            if (index > 24)
                index = 0;
        }


        public Bitmap Background { get; }

        public Bitmap UnitWriteableBitmap
        {
            get => _unitWriteableBitmap;
            private set => this.RaiseAndSetIfChanged(ref _unitWriteableBitmap, value);
        }

        public Bitmap AuraWriteableBitmap
        {
            get => _auraWriteableBitmap;
            private set => this.RaiseAndSetIfChanged(ref _auraWriteableBitmap, value);
        }

        public Bitmap UnitResavedBitmap
        {
            get => _unitResavedBitmap;
            private set => this.RaiseAndSetIfChanged(ref _unitResavedBitmap, value);
        }

        public Bitmap AuraResavedBitmap
        {
            get => _auraResavedBitmap;
            private set => this.RaiseAndSetIfChanged(ref _auraResavedBitmap, value);
        }


        private static Bitmap[] GetWriteableBitmaps(string path)
        {
            var bitmaps = new Bitmap[FRAMES_COUNT];

            for (int i = 0; i < FRAMES_COUNT; ++i)
            {
                // fixed typo
                //var bitmap = new WriteableBitmap(WIDTH, HEIGHT, PixelFormat.Rgba8888);
                var bitmap = new WritableBitmap(WIDTH, HEIGHT, PixelFormat.Rgba8888);
                var data = File.ReadAllBytes($"{path}{i}");

                using (var l = bitmap.Lock())
                {
                    for (int row = 0; row < HEIGHT; ++row)
                    {
                        var begin = (row * WIDTH) << 2;
                        var length = WIDTH << 2;

                        Marshal.Copy(data, begin, new IntPtr(l.Address.ToInt64() + row * length), length);
                    }
                }

                bitmaps[i] = bitmap;
            }

            return bitmaps;
        }

        private static Bitmap[] GetResavedBitmap(Bitmap[] originalBitmaps)
        {
            var resavedBitmaps = new List<Bitmap>(originalBitmaps.Length);

            foreach (var originalBitmap in originalBitmaps)
            {
                using (var memoryStream = new MemoryStream())
                {
                    originalBitmap.Save(memoryStream);

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    var resavedBitmap = new Bitmap(memoryStream);
                    resavedBitmaps.Add(resavedBitmap);
                }
            }

            return resavedBitmaps.ToArray();
        }
    }
}
