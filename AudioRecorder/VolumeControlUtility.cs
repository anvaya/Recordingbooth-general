using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using NAudio.Mixer;

namespace AudioRecorder
{
    /// <summary>
    /// Utility class to manage volume controls
    /// </summary>
    public class VolumeControlUtility
    {

        private static UnsignedMixerControl ScanControls(MixerLine mixerLine)
        {
            foreach (MixerControl control in mixerLine.Controls)
            {
                if (control.ControlType == MixerControlType.Volume)
                {
                    return (control as UnsignedMixerControl);
                }
            }

            // None found
            return (null);
        }

        public static UnsignedMixerControl FindVolumeControl(WaveIn waveIn)
        {
            int waveInDeviceNumber = waveIn.DeviceNumber;

            if (Environment.OSVersion.Version.Major >= 6) // Vista and over
            {
                MixerLine mixerLine = waveIn.GetMixerLine();

                return (ScanControls(mixerLine));
            }
            else
            {
                var mixer = new Mixer(waveInDeviceNumber);

                // Scan destinations ...
                foreach (var destination in mixer.Destinations)
                {

                    // ... for WaveIn devices
                    if (destination.ComponentType == MixerLineComponentType.DestinationWaveIn)
                    {

                        // Scan the sources ...
                        foreach (var source in destination.Sources)
                        {

                            // ... for microphones
                            if (source.ComponentType == MixerLineComponentType.SourceMicrophone)
                            {
                                MixerLine mixerLine = source as MixerLine;

                                return (ScanControls(mixerLine));
                            }
                        }
                    }
                }

                // Nothing found
                return (null);
            }

        }


    }
}
