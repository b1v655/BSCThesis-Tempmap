   M S B A M L   `   `   `  ����   WpfApp2    WpfApp2.View.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Hőtérkép készítő��$	��640��$	��480��$	��480��$	��640��.��� -    �Name$ mygrid��$��-2,0,2,0q� � RowDefinitions �� $
��Auto �$��20���� �� $
��Auto �.+�o� $��0��.@�m� $��Fájl��.@�m� $��Új térkép�� m� Command �� *NewMapCommand+m� $��Nézet��.@�m� $��Hideg szín�� m� InputGestureText$ Ctrl+1�� �� *SetFirstColorCommand+m� $��Közepesen hideg szín��$ Ctrl+2�� �� *SetSecondColorCommand+m� $��Közepes szín��$ Ctrl+3�� �� *SetThirdColorCommand+m� $��Közepesen meleg szín��$ Ctrl+4�� �� *SetForthColorCommand+m� $��Meleg szín��$ Ctrl+5�� �� *SetFifthColorCommand+m� $��Legmelegebb szín��$ Ctrl+6�� �� *SetSixthColorCommand+m� $��Összes szín��$ Ctrl+0�� �� *SetAllColorCommand+�� m� $��Frissítés��$ Ctrl+F�� �� *RefreshCommand+�� m� $��	3D nézet�� �� *ThreeDViewCommand+m� $��Mentés��.@�m� $��Kép mentése��$ Ctrl+S�� �� *SaveImageCommand+�� m� $��Pontok mentése��$ Ctrl+Shift+S�� �� *SavePointsCommand+m� $��Pontok betöltése��$ Ctrl+Shift+L�� �� *LoadPointsCommand+�� m� $��Beállítások��.@�m� $��Mátrix felbontás��.@�m� $��Cholesky felbontás�� �� *CholeskyCommand+m� $��Gauss elimináció�� �� *	GECommand+m� $;��5Gauss elimináció részleges főelemkiválasztással�� �� *GESPPCommand+m� $��Bázisfüggvény típusa��.@�m� $��Multikvadrik�� �� *	MQCommand+m� $��Inverzkvadratik�� �� *	IQCommand+m� $��Gaussi�� �� *
GCommand+m� $��Inverz-multikvadrik�� �� *
IMQCommand+m� $��Thin plate spline�� �� *
TPSCommand+m� $��Fügvényfolytonosság��.@�m� $��	Folytonos�� �� *
CABCommand+m� $��Kerekített�� �� *NCABCommand+m� $��Epszilon értéke��.@�m� $��0,005�� �� *EpsylonFiveCommand+m� $��0,001�� �� *EpsylonOneCommand+m� $��0,0005�� �� *EpsylonNullFiveCommand+m� $��0,0001�� �� *EpsylonNullOneCommand+�� -    ��Name$ ParentCv��$��1��$��0��$
��Left=�$	��Top=��� -   $ colorsCV��$��1��$��34��$	��297��$��Right=�$��Bottom=��� -    ��Name$ MyStatusBar��$��2��$��Stretch=�$��25��$��Bottom=�.@��� .���� -   $��StatusBarText���� $��Center=�.���� -    ��Name$ StatusBarText2��$��Center=��� $��Right=�.���� -   $ StatusBarText3��$��Right=� J� InputBindings d� 	 d� Gesture$	 LeftDoubleClicka�
 �� Command
 �� *PointPasteCommand+��  �� 	Modifiers$
 Ctrlf�
 �� Key$ D1��
 �� *SetFirstColorCommand+�� $
 Ctrlf�$ D2��
 �� *SetSecondColorCommand+�� $
 Ctrlf�$ D3��
 �� *SetThirdColorCommand+�� $
 Ctrlf�$ D4��
 �� *SetForthColorCommand+�� $
 Ctrlf�$ D5��
 �� *SetFifthColorCommand+�� $
 Ctrlf�$ D6��
 �� *SetSixthColorCommand+�� $
 Ctrlf�$ D0��
 �� *SetAllColorCommand+�� $
 Ctrlf�$ F��
 �� *RefreshCommand+�� $
 Ctrlf�$ S��
 �� *SaveImageCommand+�� $ Delete��
 �� *PointRemoveCommand+�� $ 
Ctrl+Shiftf�$ S��
 �� *SavePointsCommand+�� $ 
Ctrl+Shiftf�$ L��
 �� *LoadPointsCommand+d� $	 	LeftClicka�
 �� *ChoosedPointCommand+