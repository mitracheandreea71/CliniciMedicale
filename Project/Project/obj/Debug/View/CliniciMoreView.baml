   M S B A M L   `   `   `  ����B  >Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Project.ViewModelProject.ViewModel  5      +clr-namespace:Project.ViewProject.View  5      "    Project.View.CliniciMoreView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .vmclr-namespace:Project.ViewModel     5      ,localclr-namespace:Project.View     5        � DataContext  5      ,  &Project.ViewModel.CliniciMoreViewModel  5   
   .��5      � ���	#FFE4E4E4 � ColumnDefinitions 5   
   �� 5      $
��326* �6   �� 5      $
��775* �6   .+�5   
   �� $
��Left=�6   $	��158��6/   $��0,10,0,0q�6<   $	��Top=�6N   $
��1100��6f   
������� �� RadiusX$ 15��6�    �� RadiusY$ 15��6�   $��2��6�   �� 5   
      RoundedComboBoxStyle#��[  6    �� SelectedIndex$ -1��6�   $��2��6�   $
��Left=�6�   $��56��6  $��
179,61,0,0q�6   $	��Top=�64  $	��743��6L  $��25(�6X   �� SelectedValue 6A   �� *
NumeOras+ �� UpdateSourceTrigger$ PropertyChanged=���6�   �� *
ListaOrase+$ PropertyChanged=�6X  �� 5   
   $
��Auto=�6   $��Disabled=�6:   $��	0,173,0,0q�6c   $��2��6v   .��5      �� $	��827��6;   $��0,0,0,0q�6H   ��6   �� *	Clinici+��5      �� 5      .R�5      �  � Orientation$ Vertical=�6$   5      ��5       �� 5!      .R�5"      �� ���	#FFE4E4E4$��1q�69    �� CornerRadius$ 15��6M   $
��1100��6_   
�������	 �� Padding$	 10q�6   ._�5#      � $��-11,0,-11,0q�6#   .+�5$   "   �� ���	#FFE4E4E4$��1q�6A   $��Center=�6U   $��1��6r   $��0,10,0,0q�6}   $	��Top=�6�   $
��1028��6�   
 J� RenderTransformOrigin$
 0.5,0.5&�6�   �� 5&   "   $	��180��6G   $	��300��6T   $��Uniform=�6`   $��12,16,803,15q�6r   ��6(   �� *CaleImagine+6r   �� 5'   "   $
��Bold"�6I   $��24(�6[   $��308,40,233,159q�6i    �� TextAlignment$ Center=�6�   $��Arial Black)�6�   ��6,   �� *NumeClinica+6�   �� 5(   "   $
��Bold"�6C   $��15(�6U   $��363,70,288,139q�6c   $ Center=�6{   $��Arial Black)�6�   ��6,   �� *Judet+6�   �� 5)   "   $��16(�6D   $��384,90,308,118q�6R   $ Center=�6j   $
��Wrap=�6�   $��21��6�   $��Arial)�6�   ��6,   �� *Adresa+6�   �� 5*   "   $
��Bold"�6E   $��14(�6W   $��363,114,288,96q�6e   $ Center=�6}   $��Arial Black)�6�   ��6,   �� *	Program+6�   �� 5+   "     RoundedButtonStyle#��[ 6)   $��Solicita Programare��6V   $��516,135,440,63q�6t   $��16(�6�   6�   �� 5,   "   ���	#FFE4E4E4$��1q�6A   $��Center=�6U   $��1��6r   $��	0,201,0,0q�6}   $	��Top=�6�   $
��1028��6�   $
 0.5,0.5&�6�   �� 5-   "   $��Specializari��6,   $
��Bold"�6@   $��24(�6R   $��909,40,36,159q�6`   $ Center=�6w   $��Arial Black)�6�   �� 5.   "     CenteredListBoxStyle#��[ 6*   $	��100��6X   $��878,66,4,63q�6e   $��Arial)�6�   
���������6z   �� *ListaSpecializari+6�   5/      50      51      53      54      �� 55   
   $
��Left=�6   $��28��6/   $��
187,33,0,0q�6;   $
��Wrap=�6O   $��Alege un Oras��6c   $	��Top=�6x   $	��139��6�   $��Arial)�6�   ���	#FF888888$��2��6�   $��20(�6�   56      57      