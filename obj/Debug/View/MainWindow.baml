   M S B A M L   `   `   `  ����  WpfApp2    WpfApp2.View.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Hőtérkép készítő��5   	   $	��640��6"   $	��480��6.   $	��480��6<   $	��640��6L   .��5      � -   5         �Name$ mygrid��5      $��-2,0,2,0q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   $��20��6*   �� 5      �� 5      $
��Auto �6   .+�5   
   o� $��0��6   .@�5      m� $��Fájl��6   .@�5      m� $��Új térkép��6    m� Command 6.   �� *NewMapCommand+5      m� 5      $��Nézet��6   .@�5      m� $��Hideg szín��6    m� InputGestureText$ Ctrl+1��6/    6I   �� *SetFirstColorCommand+m� 5      $��Közepesen hideg szín��6   $ Ctrl+2��69    6S   �� *SetSecondColorCommand+m� 5      $��Közepes szín��6   $ Ctrl+3��61    6K   �� *SetThirdColorCommand+m� 5      $��Közepesen meleg szín��6   $ Ctrl+4��69    6S   �� *SetForthColorCommand+m� 5      $��Meleg szín��6   $ Ctrl+5��6/    6I   �� *SetFifthColorCommand+m� 5      $��Legmelegebb szín��6   $ Ctrl+6��65    6O   �� *SetSixthColorCommand+m� 5      $��Összes szín��6   $ Ctrl+0��60    6J   �� *SetAllColorCommand+�� 5      m� 5      $��Frissítés��6   $ Ctrl+F��6.    6H   �� *RefreshCommand+�� 5      m� 5      $��	3D nézet��6    6-   �� *ThreeDViewCommand+5       m� 5!      $��Mentés��6   .@�5"      m� $��Kép mentése��6   $ Ctrl+S��61    6K   �� *SaveImageCommand+�� 5#      m� 5$      $��Pontok mentése��6   $ Ctrl+Shift+S��64    6T   �� *SavePointsCommand+m� 5%      $��Pontok betöltése��6   $ Ctrl+Shift+L��66    6V   �� *LoadPointsCommand+�� 5&      5(      m� 5)      $��Beállítások��6   .@�5*      m� $��Mátrix felbontás��6   .@�5+      m� $��Cholesky felbontás��6    6;   �� *CholeskyCommand+m� 5,      $��Gauss elimináció��6    69   �� *	GECommand+m� 5-      $;��5Gauss elimináció részleges főelemkiválasztással��6    6X   �� *GESPPCommand+5.      m� 5/      $��Bázisfüggvény típusa��6   .@�50      m� $��Multikvadrik��6    65   �� *	MQCommand+m� 51      $��Inverzkvadratik��6    68   �� *	IQCommand+m� 52      $��Gaussi��6    6/   �� *
GCommand+m� 53      $��Inverz-multikvadrik��6    6<   �� *
IMQCommand+m� 54      $��Thin plate spline��6    6:   �� *
TPSCommand+55      m� 56      $��Fügvényfolytonosság��6   .@�57      m� $��	Folytonos��6    62   �� *
CABCommand+m� 58      $��Kerekített��6    63   �� *NCABCommand+59      m� 5:      $��Epszilon értéke��6   .@�5;      m� $��0,005��6    6.   �� *EpsylonFiveCommand+m� 5<      $��0,001��6    6.   �� *EpsylonOneCommand+m� 5=      $��0,0005��6    6/   �� *EpsylonNullFiveCommand+m� 5>      $��0,0001��6    6/   �� *EpsylonNullOneCommand+5?      5@      5A      �� 5B   
   -   5         ��Name$ ParentCv��5B      $��1��6\   $��0��6i   $
��Left=�5C      $	��Top=�6,   5D      �� 5E   
   -   5        $ colorsCV��5E      $��1��6$   $��34��61   $	��297��6<   $��Right=�6I   $��Bottom=�6e   �� 5F   
   -   5         ��Name$ MyStatusBar��5F      $��2��6*   $��Stretch=�67   $��25��6U   $��Bottom=�6b   .@�5G      �� .��5H      �� -   5        $��StatusBarText��5H      5J      �� 5K      $��Center=�6   .��5L      �� -   5         ��Name$ StatusBarText2��5L      $��Center=�62   5M      �� 5N      $��Right=�6   .��5O      �� -   5        $ StatusBarText3��5O      $��Right=�64   5P      5Q      5S       J� InputBindings 5T      d� 5U   
   	 d� Gesture$	 LeftDoubleClicka�6   
 �� Command
 61   �� *PointPasteCommand+�� 5V   
    �� 	Modifiers$
 Ctrlf�6   
 �� Key$ D1��6&   
 6/   �� *SetFirstColorCommand+�� 5W   
   $
 Ctrlf�6   $ D2��6&   
 6/   �� *SetSecondColorCommand+�� 5X   
   $
 Ctrlf�6   $ D3��6&   
 6/   �� *SetThirdColorCommand+�� 5Y   
   $
 Ctrlf�6   $ D4��6&   
 6/   �� *SetForthColorCommand+�� 5Z   
   $
 Ctrlf�6   $ D5��6&   
 6/   �� *SetFifthColorCommand+�� 5[   
   $
 Ctrlf�6   $ D6��6&   
 6/   �� *SetSixthColorCommand+�� 5\   
   $
 Ctrlf�6   $ D0��6&   
 6/   �� *SetAllColorCommand+�� 5]   
   $
 Ctrlf�6   $ F��6&   
 6.   �� *RefreshCommand+�� 5^   
   $
 Ctrlf�6   $ S��6&   
 6.   �� *SaveImageCommand+�� 5_   
   $ Delete��6   
 6"   �� *PointRemoveCommand+�� 5`   
   $ 
Ctrl+Shiftf�6   $ S��6,   
 64   �� *SavePointsCommand+�� 5a   
   $ 
Ctrl+Shiftf�6   $ L��6,   
 64   �� *LoadPointsCommand+d� 5b   
   $	 	LeftClicka�6   
 6+   �� *ChoosedPointCommand+5f      