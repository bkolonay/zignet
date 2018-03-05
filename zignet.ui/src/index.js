import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

import ZigNetApi from './api/ZigNetApi'
import './common/bootstrap.css'

ReactDOM.render(
  <App zigNetApi={new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/')} />,
  document.getElementById('root')
);
registerServiceWorker();
