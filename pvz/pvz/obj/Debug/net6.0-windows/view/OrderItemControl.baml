   M S B A M L   `   `   `  ����E  Amvvmsample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:mvvmsample.modelmvvmsample.model  5      &     mvvmsample.view.OrderItemControl   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      0modelclr-namespace:mvvmsample.model     5        � 	Resources  5
      )   #mvvmsample.model.ImagePathConverter   ImagePathConverter��%   &	          5   
   6#   5      .��5      �� ���	#FFCFD6C9$��5q�6$    �� CornerRadius$ 10��6/   ._�5   
   � .+�5      �� $ 10��6   
������ $��1q�6A   ._�5      �� $
��Left=�6   $	��203��63   $��	10,10,0,0q�6@   $	��Top=�6S   $	��274��6k   ��6w   �� *pr_id+ �� 	Converter# [   J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ Black��6+      	Direction$	 320Q�69      ShadowDepth$ 5Q�6I      Opacity$	 0.5Q�6Y      
BlurRadius$ 5Q�6g   	 J� OpacityMask	 5      !� 5      
 j� Stretch$

 Fill=�6&    !� Visual 5      �� 5   "   $	��274��6,   $	��203��68    �� RadiusX$ 15��6E    �� RadiusY$ 15��6R   
�������6_   5      5      5      �� 5      -   5         ��Name$ Id_Label��5      $
��Left=�6?   $��
74,220,0,0q�6Z   $	��Top=�6n   $��48(�6�   $��Bahnschrift SemiBold)�6�   $
��Bold"�6�    J� RenderTransformOrigin$ 0.522,1.545&�6�   ���	#FF161C1B � Cursor$
 None��6  ��6&   �� *O_id+��6�   Z� 6  �� 5      -   5        $ productName��5      $
��Left=�6F   $��
286,10,0,0q�6a   $	��Top=�6u   $��Bahnschrift SemiBold)�6�   $��22(�6�   ���	#FF161C1B$��Normal"�6�   ��6)   �� *	pr_name+��6�   Z� 6�   ]� 5      $��286,46,-286,0q�6   .+�5       �� -   5        $ ClientName_text��5       $��Имя клиента:��61   $��Center=�6H   $	��Top=�6e   $��22(�6}   $��Bahnschrift SemiBold)�6�   ���	#FF161C1B6�   �� 5!      -   5        $ 
clientName��5!      $��Right=�6I   $��-7,0,0,0q�6`   $
��Left=�6r   $	��Top=�6�   $��22(�6�   $��Bahnschrift SemiBold)�6�   ���	#FF161C1B��6,   �� *
username+6�   5"      �� 5#      -   5        $ 
stattusext��5#      $��Статус: ��6(   $
��Left=�6;   $��
286,82,0,0q�6V   $	��Top=�6j   $��Bahnschrift SemiBold)�6�   $��22(�6�   ���	#FF161C1B6�   �� 5$      -   5        $ statusLabel��5$      $
��Left=�6D   $��
374,82,0,0q�6_   $	��Top=�6s   $��22(�6�   $��Bahnschrift SemiBold)�6�   ���	#FF161C1B��6)   �� *Status+6�   ]� 5&      $��286,145,0,0q�6   .+�5'      �� -   5        $ 	CostLabel��5'      $
��Left=�6G   $
��Left=�6]   $	��Top=�6x   $$��Bahnschrift SemiBold Condensed)�6�   $��48(�6�   $��Normal"�6�   ���	#FF161C1B��6+   �� *	pr_cost+��6�   Z� 6�   �� 5(      -   5        $	 rub��5(      $��руб��6%   $
��Left=�63   $��-7,0,0,0q�6I   $
��Left=�6[   $	��Top=�6v   $$��Bahnschrift SemiBold Condensed)�6�   $��48(�6�   $��Normal"�6�   ���	#FF161C1B��6�   Z� 6�   5)      �� 5*      -	   5         ��Name$ IssueButton��5*      $��ВЫДАТЬ��5+      $
��Left=�5,      $��536,220,0,0q�5-      $	��Top=�5.      $��72��5/      $	��129��50      ���	#FF206912$��Bahnschrift SemiBold)�52      $��Normal"�53      $��20(�54      ���	#FFCFD6C9��56      Z� ��57      �� *DataContext.IssueCommand+ �� RelativeSource �� *FindAncestor+ �� AncestorType ListView��58      �� *O_id+��59      �� 5:       �� 
TargetType" ��6   .�5;      ��  �� Property � �� �� Value �	#FF20691267   �� 5<       � �� 5=      �� 5>   "    �� 
TargetType" ��62   .R�5?   &   �� #��z��6-   #��z��6W   #��z��6�   ._�5@   *   �� $��Center=�6;   $��Center=�6X   5A   '   5B   #   5D       �� Triggers 5E      P� 5F       P� Property �@J�IsMouseOver P� Value$
 True��6=   . �5G   "   ��  � �� �	#FF2069126?   5H      5J      5L      �� 5N      $��ВОЗВРАТ��6   $
��Left=�5O      $��663,220,0,0q�5P      $	��Top=�5Q      $��72��5R      $	��129��5S      ���	#FF691220$��20(�5U      $��Bahnschrift SemiBold)�5V      ���	#FFCFD6C9��5W      Z� ��5Y      �� *DataContext.ReturnCommand+ �� *FindAncestor+ ListView��5Z      �� *O_id+��5[      �� 5\      " ��6   .�5]      ��  � �� 5^      �� 5_   "   " ��62   .R�5`   &   �� #��z��6-   #��z��6W   #��z��6�   $ 0,0,10,0��6�   ._�5a   *   �� $��Center=�6;   $��Center=�6X   5b   '   5c   #   5e       5f      P� 5g       �@J�IsMouseOver$
 True��6=   . �5h   "   ��  � �� �	#FF6912206?   5i      5k      5m      �� 5o      -
   5        $ Id_Label_text��5o      $	��ID:��6+   $
��Left=�69   $��
10,218,0,0q�6T   $	��Top=�6h   $��48(�6�   $��Bahnschrift SemiBold)�6�   $
��Bold"�6�   $ 0.522,1.545&�6�   ���	#FF161C1B$
 None��6  ��6�   Z� 6  5q      5r      5s      