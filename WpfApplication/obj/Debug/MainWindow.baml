   M S B A M L   `   `   `  ����I  EWpfApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:ClientApplicationClientApplication  5   	   "    ClientApplication.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:ClientApplication     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � ���	#FF34801D.+�5
   
   G� -   5         G�Name$ password_box��5
      $
��Left=�6,   $��	98,74,0,0q�6G   $	��Top=�6Z   $	��143��6r    J� RenderTransformOrigin$ 0.319,-3.833&�6~   �� 5   
   -   5         ��Name$ user_name_texbox��5      $
��Left=�6,   $��22��6G   $��	98,27,0,0q�6S    �� TextWrapping$
 Wrap=�6f   $	��Top=�6z   $	��143��6�   �� 5   
   -   5         ��Name$ button_path_upload��5      $��Browse��6-   $
��Left=�6>   $��416,154,0,0q�6Y   $	��Top=�6n   $��91��6�   $ 
3.493,0.75&�6�   $��22��6�   ���	#FF22C75E6�   �� 5   
   -   5         ��Name$ user_name_label��5      $��User name: ��6)   $
��Left=�6?   $��	21,23,0,0q�6Z   $	��Top=�6m   �� 5   
   -   5        $ password_label��5      $��Password��6(   $
��Left=�6;   $��	21,66,0,0q�6V   $	��Top=�6i   $��72��6�   �� 5   
   -   5        $ upload_path_label��5      $��Upload path:��6+   $
��Left=�6B   $��
42,128,0,0q�6]   $	��Top=�6q   �� 5   
   -   5        $ download_path_label��5      $��Download path:��6-   $
��Left=�6F   $��
42,181,0,0q�6a   $	��Top=�6u   $ 0.486,5.346&�6�   �� 5   
   -   5        $ upload_path_texbox��5      $
��Left=�6.   $��22��6I   $��
42,154,0,0q�6U   $
 Wrap=�6i   $	��Top=�6}   $	��363��6�   �� 5   
   -	   5        $ download_path_texbox��5      $
��Left=�60   $��22��6K   $��
42,205,0,0q�6W   $
 Wrap=�6k   $	��Top=�6   $	��363��6�   �� 5   
   -
   5        $ button_path_download��5      $��Browse��6/   $
��Left=�6@   $��416,205,0,0q�6[   $	��Top=�6p   $��91��6�   $ 
3.493,0.75&�6�   $��22��6�   ���	#FF22C75E6�   �� 5   
   -   5         ��Name$ success_upload��5      $��13,154,480,141q�6(   $��success.png��6@   $
��Fill=�6U   $ -0.25,0.083&�6d   $y�Hidden=�6�   �� 5   
   -   5        $ success_download��5      $��13,205,480,90q�6*   $��success.png��6A   $
��Fill=�6V   $y�Hidden=�6e   �� 5   
   -   5        $ update_button��5      $��Update��6(   $
��Left=�69   $��122,251,0,0q�6T   $	��Top=�6i   $	��236��6�   $ 
1.267,0.65&�6�   $��36��6�   ���	#FF22C75E6�   5      5      