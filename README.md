# FanControl.HWInfoReg

Plugin for [FanControl] (https://github.com/Rem0o/FanControl.Releases) that 
provides support for HWInfo sensors using its the registry export feature of 
HWInfo.

## To install

1. Go to HWInfo settings -> HWiNFO Gadget tab. Check "Report value in Gadget" 
for the Fan and Temp sensors you need in FanControl. 
2. Compile the solution.
3. Copy the bin/release content into FanControl's "plugins" folder.
4. (Re)Start FanControl

Important notes: 
* Tested with HWInfo 7.02
* If you change which sensors are being exported in HWiNFO you'll
  need to restart FanControl or you might get unexpected results.
* If you see a -999 temperature somewhere it means that the plugin cannot read
  the sensor value from the registry for some reason.


