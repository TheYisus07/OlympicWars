   M S B A M L   `   `   `  ����S  OOlympicWarsClientProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:OlympicWarsClientProjectOlympicWarsClientProject  5   	   e7clr-namespace:OlympicWarsClientProject.Properties.Langs)OlympicWarsClientProject.Properties.Langs  5   	   '    !OlympicWarsClientProject.Register   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:OlympicWarsClientProject     5   	   Llanguage7clr-namespace:OlympicWarsClientProject.Properties.Langs     5   	   _ [MaterialDesignThemes.Wpf, Version=4.3.0.3, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	   	     � Title$  RegisterUser��5
   	   $	��520��6   $	��750��6+   .��5      � -   5         �Name$ gridRegister��5      ��5   
   
� 5      .��5      �  � Color$ 	#FFFF9E58��6    � Offset$ 0Q�61   � 5      $ SaddleBrown��6   $ 1Q�63   5      .+�5   
   �� $��15,30,300,29q�6   $	��400��6'   
�������$��Center=�6I    �� Padding$ 30q�6d    �� CornerRadius$ 10��6q   $	��650��6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 30Q�6#      Color$ 	LightGray��63   	   ShadowDepth$	 0Q�6E   ._�5      �� .+�5      �� $��Welcome��6   $��30(�6+   $
��Bold"�69   $��0 0 0 20q�6K   � 5      $��0 0 0 15q�6   .+�5      �� $��Registration��6   $
��Left=�64   �� 5      -   5        $��Login��5      $��Right=�6-      MaterialDesignFlatButton#��[  6I   6{   5      �� 5      -   5        
 ��Name$
 textBoxNickname��5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Nickname��63    % !MaterialDesignFloatingHintTextBox#��[ 6]   $	��0 5q�6�   � 5!      $��0 0 0 10q�6   .+�5"      G� -   5         G�Name$ passwordBoxPassword��5"   "   $ Password��6?    ) %MaterialDesignFloatingHintPasswordBox#��[ 6i   $��0,5,92,5q�6�   �� 5#      -   5         ��Name$ buttonSeePassword��5#      $
��Left=�6Z   $��
299,12,0,0q�6u   $	��Top=�6�   $��25��6�   $��25��6�   ��6�   Z� ��6�   Z� ��5$      �� 5%       �� ImageSource$ Resources/See1.png��6)    j� Stretch$ Uniform=�6J   5'      �� 5(      -   5        $
 textBoxPassword��5(      $ Password��67   #��[ 6a   $��0,5,92,5q�6�   $y�Hidden=�6�   �� 5)      -   5        $ buttonHidePassword��5)      $
��Left=�6]   $��
299,12,0,0q�6x   $	��Top=�6�   $��25��6�   $��25��6�   $y�Hidden=�6�   ��6�   Z� ��6�   Z� ��5*      �� 5+      $ Resources/See2.png��6)   $ Uniform=�6J   5-      5/      � 51      $��0 0 0 10q�6   .+�52      G� -   5        $  passwordBoxPasswordConfirm��52   "   $ Confirm Password��6F   #��[ 6x   $��0,5,92,5q�6�   $y�Visible=�6�   �� 53      -	   5        $ buttonSeePasswordConfirm��53      $
��Left=�6h   $��
299,12,0,0q�6�   $	��Top=�6�   $��25��6�   $��25��6�   ��6�   Z� ��54      �� 55      $ Resources/See1.png��6)   $ Uniform=�6J   57      �� 58      -
   5        $
 textBoxPasswordConfirm��58      $ Confirm Password��6>   #��[ 6p   $��0,5,92,5q�6�   $y�Hidden=�6�   �� 59      -   5        $ buttonHidePasswordConfirm��59      $
��Left=�6j   $��
299,12,0,0q�6�   $	��Top=�6�   $��25��6�   $��25��6�   $y�Hidden=�6�   ��6�   Z� ��5:      �� 5;      $ Resources/See2.png��6)   $ Uniform=�6J   5=      5>      �� 5@      -   5        $
 textBoxEmail��5@      $ Email��60   #��[ 6W   $	��0 5q�6�   �� 5A      -   5        $ buttonRegister��5A      $��Register��61   $
��0 20q�6D   6R   5C      5D      � 5G   
   -   5        $ 	gridLogin��5G      $	��489��6"   $	��742��6/   ��5H      
� 5I      .��5J      � $ 	#FFFF9E58��6#   $ 0Q�65   � 5K      $ SaddleBrown��6#   $ 1Q�67   5L      .+�5N      �� $��15,59,327,59q�6   $	��400��6+   
�������$��Center=�6M   $ 30q�6h   $ 10��6u   $	��650��6�   $	��371��6�    5O        5P      $ 30Q�6'   $ 	LightGray��67   $	 0Q�6I   ._�5R      �� .+�5S      �� $��Welcome��6    $��30(�6/   $
��Bold"�6=   $��0 0 0 20q�6O   � 5U      $��0 0 0 20q�6   .+�5V      �� -   5        $��Registration��5V   !   $
��Left=�68   #��[  6S   6�   �� 5W      $��Login��6!   $��Right=�61   5X      �� 5Z      -   5        $
 textBoxNicknameLogin��5Z      $ Nickname��6<   #��[ 6f   $	��0 5q�6�   � 5[      .+�5\      G� -   5        $ passwordBoxPasswordLogin��5\   &   $ Password��6H   #��[ 6r   $��0,5,90,5q�6�   �� 5]      -   5        $
 textBoxPasswordLogin��5]   "   $ Password��6@   #��[ 6j   $��0,5,90,5q�6�   $y�	Collapsed=�6�   �� 5^      -   5        $ buttonSeePasswordLogin��5^   !   $
��Left=�6i   $��
272,10,0,0q�6�   $	��Top=�6�   $��25��6�   $��25��6�   ��6�   Z� ��6�   Z� ��5_      �� 5`   "   $ Resources/See1.png��6-   $ Uniform=�6N   5b      �� 5c      -   5        $ buttonHidePasswordLogin��5c   !   $
��Left=�6r   $��
272,10,0,0q�6�   $	��Top=�6�   $��25��6�   $��25��6�   $y�	Collapsed=�6�   ��6�   Z� ��6�   Z� ��5d      �� 5e   "   $ Resources/See2.png��6-   $ Uniform=�6N   5g      5i      �� 5k      -   5        $ buttonLogin��5k      $��Login��62   $
��0 50q�6B   6P   5m      5n      �� 5o      -   5         ��Name$ 	zeusLogin��5o      $
��Left=�6'   $	��300��6B   $��
424,92,0,0q�6O   $	��Top=�6c   $	��300��6{   $��Resources/Zeus2.png��6�   5p      �� 5r   
   -   5        $ poseidonRegister��5r      $
��Left=�6*   $	��280��6E   $��448,117,0,0q�6R   $	��Top=�6g   $	��280��6   $��Resources/Poseidon.png��6�   $y�	Collapsed=�6�   � 5�   
   -   5        $ gridInputBox��5�      $y�	Collapsed=�6%   .+�5�      � 
���   � J� Opacity$	 0.5Q�6&   �� 5�      $	��250��6   
��� ���
���   �$��1q�6L   $ 	0,55,0,55��6`   $��Center=�6y   $��Center=�6�   $	��297��6�   ._�5�      �� $	��403��6   .+�5�      �� $��5q�6    $5��/Enter the confirmation code sent to your email:��6+   $
��Bold"�6b   $��Cambria)�6t   �� 5�      -   5        $
 textBoxCodeCofirmation��5�   e   $��Center=�6   $��Center=�6;   $	��200��6V   $��
56,0,141,0q�6�   $	��206��6�   �� 5�      -   5         ��Name$ textBlockCodeConfirm��5�       $y�	Collapsed=�6>   �� 5�       �� Orientation$ 
Horizontal=�6!   $��Center=�6:   .+�5�      �� -   5        $ buttonOk��5�   !   $��5q�63   $��Ok��6>   ��6K   Z� 6a   �� 5�      -   5        $ buttonCancel��5�   !   $��5q�67   $��Cancel��6B   ��6S   Z� 6i   5�      5�      5�      5�      5�      5�      