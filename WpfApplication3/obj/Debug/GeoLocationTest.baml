   M S B A M L   `   `   `  ����d  `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF  5   	    WpfApplication3%   WpfApplication3.GeoLocationTest   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     5   	     � Title$  GeoLocationTest��5   	   $	��600��6!   $
��1200��6.    � 	Resources 5         styleDosyaExpander �� 
TargetType �� Property �� Value��%  &	        �� 5   
   " 9�6+   .�5      ��  �@9�
IsExpanded . 6+   �� 5	       � ��
 �����6+   �� 5
       �@9�ExpandDirection$ Right=�60   �� 5       � ��
 �����6,   �� 5       � ��$ 0 1 1 1q�60   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   9� -   5        $��expanderAddresses��5      $��0��6,   #��[  6<   ��5      �� 5      -   5        $��txtExpanderHeader��5   ,   $��	Genişlet��6    � LayoutTransform 5      �� 5       �� Angle$ 90Q�6*   5      .��5      �� -   5        $��lstBoxAddresses��5   B   $��
0 20 20 20q�6   $	��Top=�6*   $	��400��6Y   $	��525��6e   5      5      '   !Microsoft.Maps.MapControl.WPF.Map  5    
   -   5        $��MyMap��5    e   $��1��6   +   %Microsoft.Maps.MapControl.WPF.MapCore   Center<   6Microsoft.Maps.MapControl.WPF.Design.LocationConverter$. (41.024525784454, 29.0050039343242,0.0000 6    	   	ZoomLevel$	 11.000Q�6R   
   CredentialsProviderT   NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F
 @AsTa4r991xLVZ7qJ9o2mJ374XZZyNxob6tJ2rJkzKqn2xNR7eifHRhsWhckgtkin 6r      Children 5!      ,   &Microsoft.Maps.MapControl.WPF.MapLayer  5"      -   5        $��mapTracksLayer��5"        5#      -   5        $��mapShapeLayer��5#        5$      -   5        $��mapPushPinLayer��5$      5&      �� 5'   
   -   5        $��myCanvas��5'   !   $��1��6   6!   5(      5)      