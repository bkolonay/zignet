import React, { Component } from 'react';
import { getUrlParams } from '../common/routing/SearchFilter.js'

class HomeLink extends Component {

  render() {
    return (
      <a href={"/?" + getUrlParams(this.props.filter)}>ZigNet</a>
    );
  }
}

export default HomeLink;
