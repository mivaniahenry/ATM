   M S B A M L   `   `   `  ����M  IGuichetAutomatique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:GuichetAutomatiqueGuichetAutomatique  5   	   #    GuichetAutomatique.InfoClient   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:GuichetAutomatique     5   	     � Title$  Informations Client��5   	   $	��450��6%   $	��800��62    � 	Resources 5	         ButtonStyle �� 
TargetType �� Property �� Value  DataGridStyle&   System.Windows.Controls.DataGrid��%�  &	        &	 �     �� 5
   
   " ��6$   .�5      ��  � ��$	 200��6&   �� 5       � ��$	 100��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 14(�6)   �� 5       � ��$
 Bold"�6+   5      �� 5   
   "  6&   .�5      ��  �@ AutoGenerateColumns$
 True��64   �� 5       �@ 
IsReadOnly$
 True��6+   �� 5       � ��$ 10q�6'   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $"��Informations sur vos comptes��6   $��Center=�67   $��10q�6T   $��16(�6`   $
��Bold"�6n   $	��Top=�6�     5   
   -   5          Name$ dgInfoClient��5      $��1��6)   #��[ 66   5       �� 5!   
    �� Orientation$ 
Horizontal=�6   $��Center=�6.   $��2��6K   $��10q�6X   .+�5"      �� -   5         ��Name$ btnRetourInfoClient��5"      $��Retour��62   #��[  6C   6h   �� 5#      -   5        $ Quitter��5#      $��Reprendre votre carte��6&   #��[  6F   6k   5$      5%      5&      