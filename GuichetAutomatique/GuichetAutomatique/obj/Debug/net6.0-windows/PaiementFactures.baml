   M S B A M L   `   `   `  ����M  IGuichetAutomatique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:GuichetAutomatiqueGuichetAutomatique  5   	   )    #GuichetAutomatique.PaiementFactures   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:GuichetAutomatique     5   	     � Title$  Paiement de Factures��5   	   $	��450��6&   $	��800��63    � 	Resources 5	         ButtonStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5
   
   " ��6$   .�5      ��  � ��$	 200��6&   �� 5       � ��$	 100��6'   �� 5       � ��$ 10q�6'   �� 5       � ��$ 14(�6)   �� 5       � ��$
 Bold"�6+   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $,��&Veuillez choisir la facture à payer :��6   $��Center=�6@   $��10q�6]   $��16(�6i   $
��Bold"�6w   $	��Top=�6�   $��0��6�   �� 5   
   $��1��6    �� Orientation$ Vertical=�6"   $��Center=�69   $��Center=�6V   .+�5      �� $��No. Compte :��6   $��Center=�6+   �� 5      -   5         ��Name$ 
cptProvPay��5      $	��300��6+    �� DisplayMemberPath$ IdCompte��67   	 �� SelectedValuePath$	 IdCompte��6T   �� 5      $��Facture à payer :��6   $��Center=�60   �� 5      -   5        $ 
cptDestPay��5      $	��300��6+   $ 	IdFacture��67   $	 	IdFacture��6U   �� 5      $��	Montant :��6   $��Center=�6(   �� 5      -   5        
 ��Name$
 txtMontantFacture��5      $	��150��61    �� TextWrapping$
 Wrap=�6=   �� 5      $ 
Horizontal=�6   $��Center=�62   .+�5       �� -   5         ��Name$ 
OKPaiement��5       $��OK��6-   #��[  6:   6_   �� 5!      -   5        $ AnnulerPaiement��5!      $��Annuler��62   #��[  6D   6i   5"      5#      �� 5$   
   -   5        $ Quitter��5$      $��Reprendre votre carte��6"   #��[  6B   $��Right=�6g   $	��Top=�6�   $��10q�6�   $��50��6�   $	��210��6�   6�   5%      5&      