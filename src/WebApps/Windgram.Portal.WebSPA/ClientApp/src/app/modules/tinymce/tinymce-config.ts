export const TINYMCE_CONFIG_DEFAULT = {
  resize: true,
  height: '640px',
  fontsize_formats: '12px 14px 16px 18px 24px 36px',
  plugins: [
    'preview advlist autolink link lists hr table emoticons',
  ],
  toolbar: `insertfile undo redo |
  fontsizeselect |
  bold italic underline strikethrough forecolor backcolor |
  alignleft aligncenter alignright alignjustify |
  bullist numlist outdent indent |
  emoticons medias preview`,
  content_style: `p { padding:0; margin:0;}`
};

