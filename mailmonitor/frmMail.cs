using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using iOfficeMail.POP3;
using iOfficeMail.MIMEParser;

namespace MailMonitor
{
	public class frmMail : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lblSender;
		private System.Windows.Forms.TextBox txtSender;
		private System.Windows.Forms.Label lblSubject;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.Label lblAttachments;
		private System.Windows.Forms.ListView lvwAttachments;
		private System.Windows.Forms.StatusBar sbrMain;
		private System.Windows.Forms.StatusBarPanel sbpSize;
		private System.Windows.Forms.StatusBarPanel sbpSentTime;
		private POPClient popClient;
		private Settings _settings;
		private iOfficeMail.MIMEParser.Message _msg;
		private MailBox _mailBox;
		private AxSHDocVw.AxWebBrowser wbBody;
		private string _messageID;
		private System.Windows.Forms.ImageList imlMessage;
		private System.Windows.Forms.SaveFileDialog dlgSave;
		private int _messageIndex;
		private System.Windows.Forms.MainMenu mmuMail;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem mnuSaveAs;
		private System.Windows.Forms.MenuItem mnuHR;
		private System.Windows.Forms.MenuItem mnuHR2;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private string _file=null;


		#region Entry
		public frmMail()
		{
			InitializeComponent();
		}

		public frmMail(string strFile)
		{
			InitializeComponent();
			_file=strFile;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void frmMail_Load(object sender, System.EventArgs e)
		{
			wbBody.CtlWidth=this.Width-wbBody.Left*2;
			wbBody.Width=this.Width-wbBody.Left*2;
			GetMailInfo();
		}

		#endregion

		#region Windows
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMail));
			this.lblSender = new System.Windows.Forms.Label();
			this.txtSender = new System.Windows.Forms.TextBox();
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.lblSubject = new System.Windows.Forms.Label();
			this.lblAttachments = new System.Windows.Forms.Label();
			this.lvwAttachments = new System.Windows.Forms.ListView();
			this.imlMessage = new System.Windows.Forms.ImageList(this.components);
			this.wbBody = new AxSHDocVw.AxWebBrowser();
			this.sbrMain = new System.Windows.Forms.StatusBar();
			this.sbpSize = new System.Windows.Forms.StatusBarPanel();
			this.sbpSentTime = new System.Windows.Forms.StatusBarPanel();
			this.dlgSave = new System.Windows.Forms.SaveFileDialog();
			this.mmuMail = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuSaveAs = new System.Windows.Forms.MenuItem();
			this.mnuHR = new System.Windows.Forms.MenuItem();
			this.mnuHR2 = new System.Windows.Forms.MenuItem();
			this.mnuDelete = new System.Windows.Forms.MenuItem();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.wbBody)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpSize)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpSentTime)).BeginInit();
			this.SuspendLayout();
			// 
			// lblSender
			// 
			this.lblSender.AutoSize = true;
			this.lblSender.Location = new System.Drawing.Point(8, 32);
			this.lblSender.Name = "lblSender";
			this.lblSender.Size = new System.Drawing.Size(48, 17);
			this.lblSender.TabIndex = 0;
			this.lblSender.Text = "Sender:";
			// 
			// txtSender
			// 
			this.txtSender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSender.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSender.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.txtSender.Location = new System.Drawing.Point(80, 32);
			this.txtSender.Name = "txtSender";
			this.txtSender.ReadOnly = true;
			this.txtSender.Size = new System.Drawing.Size(416, 21);
			this.txtSender.TabIndex = 1;
			this.txtSender.Text = "";
			// 
			// txtSubject
			// 
			this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSubject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.txtSubject.Location = new System.Drawing.Point(80, 8);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.ReadOnly = true;
			this.txtSubject.Size = new System.Drawing.Size(416, 21);
			this.txtSubject.TabIndex = 3;
			this.txtSubject.Text = "";
			// 
			// lblSubject
			// 
			this.lblSubject.AutoSize = true;
			this.lblSubject.Location = new System.Drawing.Point(8, 8);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(54, 17);
			this.lblSubject.TabIndex = 2;
			this.lblSubject.Text = "Subject:";
			// 
			// lblAttachments
			// 
			this.lblAttachments.AutoSize = true;
			this.lblAttachments.Location = new System.Drawing.Point(8, 56);
			this.lblAttachments.Name = "lblAttachments";
			this.lblAttachments.Size = new System.Drawing.Size(79, 17);
			this.lblAttachments.TabIndex = 6;
			this.lblAttachments.Text = "Attachments:";
			// 
			// lvwAttachments
			// 
			this.lvwAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lvwAttachments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvwAttachments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvwAttachments.HideSelection = false;
			this.lvwAttachments.HoverSelection = true;
			this.lvwAttachments.Location = new System.Drawing.Point(80, 56);
			this.lvwAttachments.Name = "lvwAttachments";
			this.lvwAttachments.Size = new System.Drawing.Size(416, 21);
			this.lvwAttachments.SmallImageList = this.imlMessage;
			this.lvwAttachments.TabIndex = 7;
			this.lvwAttachments.View = System.Windows.Forms.View.List;
			this.lvwAttachments.DoubleClick += new System.EventHandler(this.lvwAttachments_DoubleClick);
			// 
			// imlMessage
			// 
			this.imlMessage.ImageSize = new System.Drawing.Size(16, 16);
			this.imlMessage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlMessage.ImageStream")));
			this.imlMessage.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// wbBody
			// 
			this.wbBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.wbBody.Enabled = true;
			this.wbBody.Location = new System.Drawing.Point(8, 88);
			this.wbBody.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wbBody.OcxState")));
			this.wbBody.Size = new System.Drawing.Size(488, 160);
			this.wbBody.TabIndex = 8;
			// 
			// sbrMain
			// 
			this.sbrMain.Location = new System.Drawing.Point(0, 231);
			this.sbrMain.Name = "sbrMain";
			this.sbrMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																					   this.sbpSize,
																					   this.sbpSentTime});
			this.sbrMain.ShowPanels = true;
			this.sbrMain.Size = new System.Drawing.Size(504, 22);
			this.sbrMain.TabIndex = 9;
			this.sbrMain.Text = "Welcome!";
			// 
			// sbpSize
			// 
			this.sbpSize.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.sbpSize.MinWidth = 120;
			this.sbpSize.Width = 120;
			// 
			// sbpSentTime
			// 
			this.sbpSentTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.sbpSentTime.MinWidth = 120;
			this.sbpSentTime.Width = 368;
			// 
			// mmuMail
			// 
			this.mmuMail.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFile});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuOpen,
																					this.mnuHR,
																					this.mnuSaveAs,
																					this.mnuHR2,
																					this.mnuDelete});
			this.mnuFile.Text = "&File";
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 0;
			this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuOpen.Text = "&Open";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Index = 2;
			this.mnuSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuSaveAs.Text = "&Save As";
			this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// mnuHR
			// 
			this.mnuHR.Index = 1;
			this.mnuHR.Text = "-";
			// 
			// mnuHR2
			// 
			this.mnuHR2.Index = 3;
			this.mnuHR2.Text = "-";
			// 
			// mnuDelete
			// 
			this.mnuDelete.Index = 4;
			this.mnuDelete.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
			this.mnuDelete.Text = "&Delete";
			this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
			// 
			// frmMail
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(504, 253);
			this.Controls.Add(this.lvwAttachments);
			this.Controls.Add(this.sbrMain);
			this.Controls.Add(this.lblAttachments);
			this.Controls.Add(this.txtSubject);
			this.Controls.Add(this.lblSubject);
			this.Controls.Add(this.txtSender);
			this.Controls.Add(this.lblSender);
			this.Controls.Add(this.wbBody);
			this.Menu = this.mmuMail;
			this.Name = "frmMail";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Message";
			this.Load += new System.EventHandler(this.frmMail_Load);
			((System.ComponentModel.ISupportInitialize)(this.wbBody)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpSize)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.sbpSentTime)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Functions
		public string EMLFile
		{
			set{_file=value;}
		}

		public MailBox MailBox
		{
			set{_mailBox=value;}
		}

		public POPClient POPClient
		{
			set{popClient=value;}
		}

		public Settings Settings
		{
			set{_settings=value;}
		}

		public string MessageID
		{
			set{_messageID=value;}
		}

		public int MessageIndex
		{
			set{_messageIndex=value;}
		}
		
		private bool FindLocalMessage(ref string strFile)
		{
			for(int i=0;i<_settings.MessageIDs.Count;i++)
			{
				if(((MailInfo)_settings.MessageIDs[i]).ID==_messageID)
				{
					strFile=_settings.GetMessageFile(_messageID);
					return true;
				}
			}

			return false;
		}

		private void GetMailInfo()
		{
			try
			{
				if(File.Exists(_file)||FindLocalMessage(ref _file))
				{
					bool blnRet=false;
					_msg=new iOfficeMail.MIMEParser.Message(ref blnRet,"",true,false,_file);
				}
				else
				{
					//if(!popClient.Connected)
					//{
						popClient.ReceiveContentSleepInterval=1;
						popClient.WaitForResponseInterval=10;				
						popClient.Connect(_mailBox.ServerAddress,_mailBox.Port);
						popClient.Authenticate(_mailBox.UserName,_mailBox.Password);
					//}
					_msg=popClient.GetMessage(_messageIndex,false);
					MailInfo mi;
					mi.ID=_msg.MessageID;
					mi.File=_settings.GetMessageFile(_msg.MessageID);
					string strPath=new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName+"\\mails";
					if(!Directory.Exists(strPath))
						Directory.CreateDirectory(strPath);
					_settings.MessageIDs.Add(mi);
					_msg.SaveToMIMEEmailFile(_settings.GetMessageFile(_msg.MessageID),true);
				}

				txtSubject.Text=_msg.Subject;
				txtSender.Text="\""+_msg.From+"\""+_msg.FromEmail;
				lvwAttachments.Items.Clear();
				ListViewItem lvi;
				for(int i=0;i<_msg.Attachments.Count;i++)
				{
					lvi=lvwAttachments.Items.Add(_msg.GetAttachmentFileName(_msg.GetAttachment(i)),1);
					lvi.Tag=i;
				}

				string strBodyFile=new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName+"\\mail.htm";
				iOfficeMail.MIMEParser.Utility.SavePlainTextToFile(strBodyFile,(string)_msg.MessageBody[_msg.MessageBody.Count-1],true);
				object o=null;
				wbBody.Navigate(strBodyFile,ref o,ref o,ref o,ref o);
				
				sbrMain.Panels[0].Text=_msg.ContentLength.ToString();
				sbrMain.Panels[1].Text=_msg.DateTimeInfo;
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,ex.Message);
			}		
		}

		private void SaveAttachment()
		{
			Attachment att=_msg.GetAttachment((int)lvwAttachments.SelectedItems[0].Tag);
			if(att!=null && _msg!=null)
			{
				dlgSave.FileName=_msg.GetAttachmentFileName(att);
				DialogResult result=dlgSave.ShowDialog();
				if(result==DialogResult.OK)
				{
					if(_msg.IsMIMEMailFile(att))
					{
						result=MessageBox.Show(this,"iOfficeMail.POP3 found the attachment is a MIME mail, do you want to extract it?","MIME mail",MessageBoxButtons.YesNo);
						if(result==DialogResult.Yes)
						{
							iOfficeMail.MIMEParser.Message  m2=att.DecodeAsMessage();
							string attachmentNames="";
							bool blnRet=false;
							if(m2.AttachmentCount>0)
								for(int i=0;i<m2.AttachmentCount;i++)
								{
									Attachment att2=m2.GetAttachment(i);
									attachmentNames+=m2.GetAttachmentFileName(att2)+"("+att2.ContentLength+" bytes)\r\n";
								}
							blnRet=_msg.SaveAttachments(System.IO.Path.GetDirectoryName(dlgSave.FileName));
							MessageBox.Show(this,"Parsing "+(blnRet==true?"succeeded":"failed")+"��\r\n\r\nsubject:"+m2.Subject+"\r\n\r\nAttachment:\r\n"+attachmentNames);
						}
						else
						{
						}
					}
					MessageBox.Show(this,"Attachment saving "+((_msg.SaveAttachment(att,dlgSave.FileName))?"succeeded":"failed")+"��");
				}
			}
			else
				MessageBox.Show(this,"attachment object is null!");		
		}

		#endregion

		#region Controls
		private void lvwAttachments_DoubleClick(object sender, EventArgs e)
		{
			SaveAttachment();
		}

		private void mnuSaveAs_Click(object sender, System.EventArgs e)
		{
			dlgSave.FileName=_msg.Subject;
			DialogResult result=dlgSave.ShowDialog();
			if(result==DialogResult.OK)			
				_msg.SaveToMIMEEmailFile(dlgSave.FileName,true);
		}

		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			dlgOpen.CheckFileExists=true;
			dlgOpen.CheckPathExists=true;
			dlgOpen.ReadOnlyChecked=false;
			if(dlgOpen.ShowDialog()==DialogResult.OK)
			{
				_file=dlgOpen.FileName;
				GetMailInfo();
			}
		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			popClient.DeleteMessage(_messageIndex);
			this.Close();
		}
		#endregion

	}
}