using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonLogin;
    public Button buttonRegister;
    public Button buttonBack;

    private bool isLogin;
    // Use this for initialization
    void Start()
    {
        isLogin = true;
        UserService.Instance.OnLogin = OnLogin;
        UserService.Instance.OnRegister = OnRegister;
    }

    private void OnEnable()
    {
        buttonLogin.onClick.AddListener(this.OnClickLogin);
        buttonRegister.onClick.AddListener(this.OnClickRegister);
        buttonBack.onClick.AddListener(this.OnBack);
    }

    private void OnDestroy()
    {
        buttonLogin.onClick.RemoveListener(this.OnClickLogin);
        buttonRegister.onClick.RemoveListener(this.OnClickRegister);
        buttonBack.onClick.RemoveListener(this.OnBack);
    }

    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        // Enter Game
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    public void OnClickRegister()
    {
        if (isLogin)
        {
            isLogin = !isLogin;
            this.ClearContent();
        }
        else
        {
            if (string.IsNullOrEmpty(this.username.text))
            {
                MessageBox.Show("请输入账号");
                return;
            }
            if (string.IsNullOrEmpty(this.password.text))
            {
                MessageBox.Show("请输入密码");
                return;
            }
            if (string.IsNullOrEmpty(this.passwordConfirm.text))
            {
                MessageBox.Show("请输入确认密码");
                return;
            }
            if (this.password.text != this.passwordConfirm.text)
            {
                MessageBox.Show("两次输入的密码不一致");
                return;
            }

            UserService.Instance.SendRegister(this.username.text, this.password.text);
        }
    }

    void OnBack()
    {
        this.isLogin = true;
        this.ClearContent();
    }

    void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
            MessageBox.Show("登录成功", "提示", MessageBoxType.Information).OnYes = this.CloseRegister;
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }


    void OnRegister(Result result, string message)
    {
        if (result == Result.Success)
            MessageBox.Show("注册成功", "提示", MessageBoxType.Information).OnYes = this.OnBack;
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }


    void CloseRegister()
    {
        ViewManager.Instance.RemoveView("UILogin");
    }

    void ClearContent()
    {
        username.text = "";
        password.text = "";
        passwordConfirm.text = "";
    }
}
