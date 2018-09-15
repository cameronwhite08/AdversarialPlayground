using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using MiniMax_TicTacToe_Lib;

namespace MasterOfTac
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, View.IOnTouchListener
	{
	    PieceType[][] currentBoard;
	    TicTacToeLib lib;
	    private int AiDepth = 15;
	    AppCompatTextView tile1, tile2, tile3, tile4, tile5, tile6, tile7, tile8, tile9;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            
		    lib = new TicTacToeLib();

		    currentBoard = lib.CreateEmptyBoard();

		    SetContentView(Resource.Layout.activity_main);

			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

		    
		    tile1 = FindViewById<AppCompatTextView>(Resource.Id.textView1);
		    tile2 = FindViewById<AppCompatTextView>(Resource.Id.textView2);
		    tile3 = FindViewById<AppCompatTextView>(Resource.Id.textView3);
		    tile4 = FindViewById<AppCompatTextView>(Resource.Id.textView4);
		    tile5 = FindViewById<AppCompatTextView>(Resource.Id.textView5);
		    tile6 = FindViewById<AppCompatTextView>(Resource.Id.textView6);
		    tile7 = FindViewById<AppCompatTextView>(Resource.Id.textView7);
		    tile8 = FindViewById<AppCompatTextView>(Resource.Id.textView8);
		    tile9 = FindViewById<AppCompatTextView>(Resource.Id.textView9);

            tile1.SetOnTouchListener(this);
            tile2.SetOnTouchListener(this);
            tile3.SetOnTouchListener(this);
            tile4.SetOnTouchListener(this);
            tile5.SetOnTouchListener(this);
            tile6.SetOnTouchListener(this);
            tile7.SetOnTouchListener(this);
            tile8.SetOnTouchListener(this);
            tile9.SetOnTouchListener(this);

			FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            reset_puzzle();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            reset_puzzle();
        }

	    private void reset_puzzle()
	    {
	        //reset the puzzle
	        currentBoard = lib.CreateEmptyBoard();

	        UpdateUI();
        }

	    private void UpdateUI()
	    {
	        tile1.Text = currentBoard[0][0] == PieceType.Empty ? "[]" : currentBoard[0][0] == PieceType.X ? "X" : "O";
            tile2.Text = currentBoard[0][1] == PieceType.Empty ? "[]" : currentBoard[0][1] == PieceType.X ? "X" : "O";
	        tile3.Text = currentBoard[0][2] == PieceType.Empty ? "[]" : currentBoard[0][2] == PieceType.X ? "X" : "O";
	        tile4.Text = currentBoard[1][0] == PieceType.Empty ? "[]" : currentBoard[1][0] == PieceType.X ? "X" : "O";
	        tile5.Text = currentBoard[1][1] == PieceType.Empty ? "[]" : currentBoard[1][1] == PieceType.X ? "X" : "O";
	        tile6.Text = currentBoard[1][2] == PieceType.Empty ? "[]" : currentBoard[1][2] == PieceType.X ? "X" : "O";
	        tile7.Text = currentBoard[2][0] == PieceType.Empty ? "[]" : currentBoard[2][0] == PieceType.X ? "X" : "O";
	        tile8.Text = currentBoard[2][1] == PieceType.Empty ? "[]" : currentBoard[2][1] == PieceType.X ? "X" : "O";
	        tile9.Text = currentBoard[2][2] == PieceType.Empty ? "[]" : currentBoard[2][2] == PieceType.X ? "X" : "O";
        }

	    public bool OnTouch(View v, MotionEvent e)
	    {
	        var r = -1;
	        var c = -1;
	        switch (v.Id)
	        {
                case Resource.Id.textView1:
                    r = 0;
                    c = 0;
                    break;
                case Resource.Id.textView2:
                    r = 0;
                    c = 1;
                    break;
                case Resource.Id.textView3:
                    r = 0;
                    c = 2;
                    break;
                case Resource.Id.textView4:
                    r = 1;
                    c = 0;
                    break;
                case Resource.Id.textView5:
                    r = 1;
                    c = 1;
                    break;
                case Resource.Id.textView6:
                    r = 1;
                    c = 2;
                    break;
                case Resource.Id.textView7:
                    r = 2;
                    c = 0;
                    break;
                case Resource.Id.textView8:
                    r = 2;
                    c = 1;
                    break;
                case Resource.Id.textView9:
                    r = 2;
                    c = 2;
                    break;
                default:
                    break;
	        }

	        if (r != -1 && c != -1 && e.Action == MotionEventActions.Down)
	        {
	            currentBoard[r][c] = PieceType.X;
	            currentBoard = lib.AI(currentBoard, PieceType.X, AiDepth);
	            UpdateUI();
	            if (lib.CheckWin(currentBoard))
	            {
	                Snackbar.Make(v, "Someone won!", Snackbar.LengthLong)
	                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                }
            }
	        return true;
	    }
	}
}

