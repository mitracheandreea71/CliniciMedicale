   M S B A M L   `   `   `  ����B  >Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Project.ViewModelProject.ViewModel  5      +clr-namespace:Project.ViewProject.View  5      !    Project.View.MediciMoreView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .vmclr-namespace:Project.ViewModel     5      ,localclr-namespace:Project.View     5        � DataContext  5      +  %Project.ViewModel.MediciMoreViewModel  5   
   .��5      � ���	#FFE4E4E4 � ColumnDefinitions 5   
   �� 5      $
��326* �6   �� 5      $
��775* �6   .+�5   
   �� $
��Left=�6   $	��158��6/   $��0,10,0,0q�6<   $	��Top=�6N   $
��1100��6f   
������� �� RadiusX$ 15��6�    �� RadiusY$ 15��6�   $��2��6�   �� 5   
   -   5         ��Name$ 	NumeMedic��5         RoundedTextBoxStyle#��[  6&   $
��Left=�6S   $��48��6n   $��	55,23,0,0q�6z    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $
��1024��6�   ���	#FF888888$��20(�6  $��2��6,  ��6�   �� *
NumeFiltru+ �� UpdateSourceTrigger$ PropertyChanged=�6,  �� 5   
   $
��Left=�6   $��29��6+   $��
714,32,0,0q�67   $	��Top=�6K   $��26��6c   $��/Images/cautare.png��6n   $��1��6�   �� 5   
   -   5         ��Name$ 
OrasCmbBox��5        RoundedComboBoxStyle#��[ 6'    �� SelectedIndex$ -1��6U   $
��Left=�6h   $��31��6�   $��
55,114,0,0q�6�   $	��Top=�6�   $	��156��6�   	 �� SelectedItem	 6�   �� *
NumeOras+$ PropertyChanged=���6  �� *
ListaOrase+$ PropertyChanged=��� 5   
   -   5        $ LocatieCmbBox��5      #��[ 6*   $ -1��6X   $
��Left=�6k   $��31��6�   $��281,114,0,0q�6�   $	��Top=�6�   $	��269��6�   $��2��6�   	 6�   �� *NumeLocatie+$ PropertyChanged=���6)  �� *	Locatii+$ PropertyChanged=��� 5   
   -   5        $ SpecialitateCmbBox��5      #��[ 6/   $ -1��6]   $
��Left=�6p   $��31��6�   $��302,114,0,0q�6�   $	��Top=�6�   $	��278��6�   $��1��6�   	 6�   �� *NumeSpecialitate+$ PropertyChanged=���6/  �� *Specialitati+$ PropertyChanged=��� 5   
   -   5        $ OrdonareCmbBox��5      #��[ 6+   $ -1��6Y   $
��Left=�6l   $��31��6�   $��632,114,0,0q�6�   $	��Top=�6�   $	��121��6�   $��1��6�   �� 5   
   $
��Left=�6   $��16��6/   $��	68,98,0,0q�6;   $
��Wrap=�6N   $
��Oras��6b   $	��Top=�6n   $��29��6�   $��Arial)�6�   ���	#FF8888886�   �� 5   
   $
��Left=�6   $��16��6/   $��
296,98,0,0q�6;   $
��Wrap=�6O   $��Locatie��6c   $	��Top=�6r   $��48��6�   $��Arial)�6�   ���	#FF888888$��2��6�   �� 5   
   $
��Left=�6   $��16��6/   $��
320,98,0,0q�6;   $
��Wrap=�6O   $��Specialitate��6c   $	��Top=�6w   $��65��6�   $��Arial)�6�   ���	#FF888888$��1��6�   �� 5   
   $
��Left=�6   $��16��6/   $��
646,98,0,0q�6;   $
��Wrap=�6O   $��Ordonare��6c   $	��Top=�6s   $��65��6�   $��Arial)�6�   ���	#FF888888$��1��6�   �� 5    
   $
��Auto=�6   $��Disabled=�6:   $��	0,173,0,0q�6c   $��2��6v   .��5!      �� $	��827��6:   $��0,0,0,0q�6G   ��6   �� *Medici+��5"      �� 5#      .R�5$      � 
 � Orientation$
 Vertical=�6$   5%      ��5(      �� 5)      .R�5*      �� ���	#FFE4E4E4$��1q�69    �� CornerRadius$ 15��6M   $
��1100��6_   
������� �� Padding$ 10q�6   ._�5+      � $��-11,0,-11,0q�6#   .+�5,   "   �� $	��200��6G   $	��150��6T   $��UniformToFill=�6`   $��
10,0,938,1q�6x   ��6(   �� *CaleImagine+6x   �� 5.   "   $
��Bold"�6I   $��24(�6[   $��224,40,154,139q�6i    �� TextAlignment$ Center=�6�   $��Arial Black)�6�   ��6,   �� *NumeComplet+6�   �� 5/   "   $
��Bold"�6E   $��14(�6W   $��363,94,288,96q�6e   $ Center=�6|   $��Arial Black)�6�   ��6,   �� *	Program+6�   �� 51   "   $��Rating: ��6,   $��24(�6<   $ Center=�6K   $��992,143,10,38q�6b   $��Arial)�6y   �� 52   "   ���	#FFE4E4E4$��1q�6A   $
��Left=�6U   $��1��6p   $��981,171,0,0q�6{   $	��Top=�6�   $	��112��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   �� 54   "   $
��Bold"�6D   $��24(�6V   $ Center=�6e   $��1017,171,33,10q�6|   $��Arial Black)�6�   ��6,   �� *Rating+6�   �� 56   "   $��16(�6D   $��384,72,308,118q�6R   $ Center=�6j   $
��Wrap=�6�   $��21��6�   $��Arial)�6�   ��6,   �� *Sectie+6�   �� 57   "     RoundedButtonStyle#��[ 6)   $��Solicita Programare��6V   $��516,120,440,59q�6t   $��16(�6�   6�   58      59      5:      5<      5=      5>      5?      