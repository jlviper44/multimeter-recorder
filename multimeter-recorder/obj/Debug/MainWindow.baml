   M S B A M L   `   `   `  ????N  Jmultimeter-recorder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:multimeter
multimeter  5      9!clr-namespace:multimeter_recordermultimeter_recorder  5          multimeter.MainWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      *localclr-namespace:multimeter     5      6controls!clr-namespace:multimeter_recorder     5      D @ModernWpf, Version=0.9.6.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.6.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   5	      0  *ModernWpf.Controls.Primitives.WindowHelper    UseModernWindowStyle  .  ? Title$ Multimeter Recorder??5      $	??363??5      $	??600??5       ? 
ResizeMode$ NoResize=?5      .??5      ?? ???#003366.+?5      ?? $??0q?6   ???#cce5ff ?? Padding$ 5q?64    ?? CornerRadius$ 7??6@   $	??310??6Q   $	??580??6^    ?? Left$ 10??6j   
 ?? Top$ 10??6{   ._?5      ?? .+?5      ?? -   5           AccentButtonStyle#??[  5      $??New File Name??5      $??Stretch=?5      $??Stretch=?5      $??32??5      $	??195??6   5      ?? 5      -   5        $??DataNameInput??5       
???????
???   ?$	 200??5      $	??370??6   5       )   #multimeter_recorder.ComPortTemplate  5"      -   5          Name$ ComPort1??5"   <   $ 71??6   $	??570??60   6<     5#      -   5        $ ComPort2??5#   <   $	 109??6   $	??570??60   6<     5$      -   5        $ ComPort3??5$   <   $	 147??6   $	??570??60   6<     5%      -   5        $ ComPort4??5%   <   $	 186??6   $	??570??60   6<   ?? 5'      
???   ?
???????$??	Duration:??5*      $??Center=?5+      $??Center=?5,      $
??Left=?5-      $??30??5.      $	??Top=?5/      $??90??50      $	 228??6   ?? 52      -   5        $??DurationValue??5<      
???   ?
???????$??Center=?55      $??Center=?56      $
??Left=?57      $??30??58      $	??Top=?59      $??90??5:      $	 228??6   $ 95??6"   $??1??5=      ?? 5?      -   5        $??DurationType??5H      
???   ?
???????$??Center=?5B      $??Center=?5C      $
??Left=?5D      $??32??5E      $??90??6   $	??Top=?5F      $	 228??5G      $	 190??6   5H      ?? 5J      
???   ?
???????$??Sample Rate:??5M      $??Center=?5N      $??Center=?5O      $
??Left=?5P      $??32??5Q      $	??Top=?5R      $??90??5S      $	 228??6   $	 285??6"   ?? 5U      -	   5        $??SampleRateValue??5`      
???   ?
???????$??Center=?5X      $??Center=?5Y      $
??Left=?5Z      $??30??5[      $??90??6   $	??Top=?5\      $	 228??5]      $	 380??6   $??1??5_      5`      ?? 5b      -
   5        $??SampleRateType??5k      
???   ?
???????$??Center=?5e      $??Center=?5f      $
??Left=?5g      $??32??5h      $??90??6   $	??Top=?5i      $	 228??5j      $	 475??6   5k      ?? 5m      -   5        $??
StopButton??5w      
???????
???  ??$
??Stop??5p      $??Center=?5q      $??Center=?5r      $
??Left=?5s      $??32??5t      $	??Top=?5u      $??90??5v      $	 267??6   5x      ?? 5z      -   5        $??StartButton??5?      #??[  5{      $??Start??5|      $??Center=?5}      $??Center=?5~      $
??Left=?5      $??32??5?      $	??Top=?5?      $	??376??5?      $	 267??6   $ 95??6#   5?      ?? 5?      -   5        #??[  5?      $??	View Data??5?      $??Center=?5?      $??Center=?5?      $
??Left=?5?      $??32??5?      $	??Top=?5?      $??95??5?      $	 267??6   $	 475??6"   5?      ?? 5?      -   5        #??[  5?      $??Refresh??5?      $??Center=?5?      $??Center=?5?      $
??Left=?5?      $??30??5?      $	??Top=?5?      $??95??5?      $ 37??6   $	 475??6!   5?      ?? 5?      -   5        $??DataSetsComboBox??5?      $??30??5?      $	??270??5?      
???????
???   ?$	 200??5?      $ 37??6   5?      ?? 5?      
???   ?
???????$??Select File Name:??5?      $??Center=?5?      $??Center=?5?      $
??Left=?5?      $??30??5?      $	??Top=?5?      $	??195??5?      $ 37??6   5?      5?      5?      5?      