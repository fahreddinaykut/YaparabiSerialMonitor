   M S B A M L   `   `   `  ���� P  LYaparabiSerialMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:YaparabiSerialMonitorYaparabiSerialMonitor  &     YaparabiSerialMonitor.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5local#clr-namespace:YaparabiSerialMonitor       � Title$  Yaparabi Serial Monitor��$	��700��$
��1000�����#1E1E1E � WindowStartupLocation$ CenterScreen=� � Icon$( "/yaparabiserialportmonitoricon.ico�� � 	Resources  �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 �� Triggers
 P� Property P� Value��%<  '��       '�� l    '��     '�� �    �� " ��.���  � �� �#2D2D2D��  � �� �#FFFFFF��  � ��$ 36����  � ��$ 1q���  � �� �#3E3E42��  � �� �� " ��.R��� #��z��#��z��#��z��$ 4��._��� $��Center=�$��Center=�	 P� 
 �@J�IsMouseOver$
 True��. ���  � �� �#404040P� 
 � ��$
 True��. ���  � �� �#505050�� " ��.���  � �� �#252526��  � �� �#D4D4D4��  � ��$ 1q���  � �� �#3E3E42��  � ��$ Consolas)���  � ��$ 13(��� " ��.���  � �� �#2D2D2D��  � �� �#FFFFFF��  � ��$ 1q���  � �� �#3E3E42��  � ��$ 36����  � ��$	 8,0q���  � ��$ Center=��� " ��.���  � �� �#D4D4D4��  � ��$ Center=�.��� $��15q� � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+��� $��0��$��24��$��24��$
��Left=�$	��Top=�$(��"/yaparabiserialportmonitoricon.png��$��0,0,0,10q��� $��0�����#252526$ 6�� �� Padding$ 15q�$��0,0,0,15q�._��  � ColumnDefinitions �� $
��Auto ��� $
��Auto ��� $��15 ��� $
��Auto ��� $��15 ��� $
��Auto ��� $��15 ��� $
��Auto ��� $��15 ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��36��$��36��$��0,0,15,0q�$��Center=�$(��"/yaparabiserialportmonitoricon.png���� $��1��.+��� -    ��Name$ portComboBox��$	��200���� -    ��Name$ portDescriptionText�����#808080$��5,2,0,0q�$��11(�$��CharacterEllipsis=�$	��200���� -   $ baudComboBox��$��3��$	��120��$��Center=�.@��� $	��300���� $
��1200���� $
��2400���� $
��4800���� $
��9600���� $��19200���� $��38400���� $��57600���� $��115200���� -    ��Name$ connectButton��$��5��$��Connect��$	��120���� -   $ resetButton��$��7��$��Reset��$	��120���� -   $ clearButton��$��9��$��Clear��$	��120���� -    ��Name$ autoScrollCheckBox��$��11��$��
Autoscroll�� d� 	IsChecked$
 TrueY��� -    ��Name$ dataTextBox��$��1��$��0,0,0,15q� �� 
IsReadOnly$
 True��" �� VerticalScrollBarVisibility$ Visible=�$ �� HorizontalScrollBarVisibility$
 Auto=�$��Consolas)�$��13(����#252526���#D4D4D4$��1q����#3E3E42$��10q�$��Stretch=�$��Stretch=�$	��200��. �/� .T�J� -	    �Name$ terminalParagraph���� $��2�����#252526$ 6��$ 15q�$��0,0,0,15q�._��  �� $��* ��� $��15 ��� $
��Auto �.+��� -
    ��Name$ sendTextBox��$��0���� -   $ 
sendButton��$��2��$��Gönder��$	��120���� $��3�����#252526$��30��$ 6��._�]� $��False��$
��15,0q�.+�A� -    A�Name$ connectionIndicator��$��10��$��10��
���  ��$
��Left=��� -   $ 
statusText��$��Bağlı Değil�����#D4D4D4$��10,0,0,0q�$��Center=�$
��Left=�