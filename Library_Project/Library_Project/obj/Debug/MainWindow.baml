   M S B A M L   `   `   `  ����J  FLibrary_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Library_ProjectLibrary_Project  5   	        Library_Project.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   )localclr-namespace:Library_Project  5   	      MaterialDesignPaper#���   5	   	     MaterialDesignBody#���  5
   	     � Title$  	HomePanel��5   	   $	��450��6   $	��800��6(    � WindowStartupLocation$ CenterScreen=�64    � WindowStyle$
 None=�6Y    � 
ResizeMode$ NoResize=�6l   .��5      � ��5   
   �� 5       �� 
StartPoint$ 0.1,0&�6"    �� EndPoint$ 0.9,1&�65   .��5      �  � Color$ 	#FF2A36AA��6    � Offset$ 1Q�61   � 5      $ 	#FF030617��6   $ 0Q�61   5      .+�5   
   �� $	��400��6   $	��800��6    �� CornerRadius$ 0,0,0,80��6*   $	��Top=�6B   ���#110E176Z   �� 5   
   	 �� Orientation$	 
Horizontal=�6   .+�5      �� $	��350��6   .+�5      �� $��/Resources/Images/New.png��6   $	��300��6;   $	��300��6G   $	��Top=�6T   $��30q�6l   $
��Fill=�6x   5      �� 5      $	��Top=�6   $	��400��6-   ���#191970$��2q�6O   $��
10,40,10,0q�6c   $	��345��6w   ._�5      �� $	��350��6   $��0,0,0,0q�6)   .+�5      �� $5��/نرم افزار مدیریت کتابخانه��6    $��
20,40,10,0q�6A   $��Calibri)�6U   
�������
 �� TextAlignment$
 Center=�6}   $��30(�6�   �� 5      $��20q�6!   $	��Top=�6-   .+�5      �� $	 
Horizontal=�6%   $��10,35,10,10q�6>   .+�5       '  !MaterialDesignThemes.Wpf.PackIcon     Kind$
 User=�66   $��25��6B   $��25��6M   
�������6Y   �� 5!      -   5         ��Name$ txtuserName��5!   ;   $��Calibri)�6&   |�. $
��10,0q�6a   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ نام کاربری��6o   
�������$	��250��6�   
������� �� 
CaretBrush �	#FFD94448 �� SelectionBrush �	#FFD94448$��15(�6   5"      �� 5#      $	 
Horizontal=�6%   $��10,50,10,10q�6>   .+�5$        $
 Lock=�66   $��25��6B   $��25��6M   
�������6Y   G� 5%      -   5         G�Name$ txtPassword��5%   ?   $��Calibri)�6*   $
��10,0q�6T   $ رمز عبور��6b   
�������$	��250��6�   
������� G� 
CaretBrush �	#FFD94448 G� SelectionBrush �	#FFD94448$��15(�6�   5&      5'      �� 5(      $	 
Horizontal=�6!   $��
60,10,20,0q�6:   .+�5)      �� -   5         ��Name$ btnCreat��5)   !   $��اکانت جدید��63   $	��100��6H   $
��Left=�6T   ���#191970
���   �$��14(�6�   $��Calibri)�6�   +  %MaterialDesignThemes.Wpf.ButtonAssist   CornerRadius$ 10��6�   $��2q�6�   
�������$��Black"�6  5*      �� 5+      -   5        $ btnLogin��5+   !   $��Calibri)�63   $��ورود��6H   $��45,0,0,0q�6W   $	��100��6i   $��Right=�6u   ���	#FF43AD03
���   �$ 10��6�   $��2q�6�   
�������$��Black"�6  6$  5,      5-      5.      � 5/      .+�50      �� -   5        $ btnClose��50      $��30��6A   $��30��6L   ���	#FFEC0C0C$	��Top=�51   2   $��Right=�6a   $��-10,10,10,0q�6}   ��50   +   Z� .��52        $ 	CloseBold=�6.   $��	-16.5,-10q�6?   $��30��6R   $��30��6]   ���	#FFEC0C0C J� RenderTransformOrigin$ 1.375,0.875&�6�   53      54      56      57      58      