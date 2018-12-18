import React, { Component } from 'react';
import Home from './Home'
import { getSuiteResults } from '../common/api/api.js';

class HomeRouter extends Component {
  constructor(props) {
    super(props);

    this.debug = false;
    if (this.props.queryString.indexOf('debug=true') !== -1)
      this.debug = true;
  }  

  render() {
    return (
      <Home getResults={() => getSuiteResults(this.debug)} />
    );
  }
}

export default HomeRouter;